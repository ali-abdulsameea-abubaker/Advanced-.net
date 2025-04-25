using Microsoft.EntityFrameworkCore;
using RealTimeChatApp.Models;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
/// <summary>
///  <Authorship>I, Ali Abdulsameea, 000857347 certify that this material is my original work.
/// No other person's work has been used without due acknowledgement.</Authorship>
///
/// 
/// <author>Ali Abubaker - 000857347</author>
/// </summary>
/// 

namespace RealTimeChatApp.Hubs;
/// <summary>
/// Handles WebSocket connections, message processing, and user management in a real-time chat application.
/// </summary>
public class WebSocketHandler
{
    private readonly ConcurrentDictionary<WebSocket, UserConnection> _connections = new();
    private readonly IServiceProvider _serviceProvider;
    /// <summary>
    /// Initializes a new instance of the <see cref="WebSocketHandler"/> class.
    /// </summary>
    /// <param name="serviceProvider">The service provider for dependency injection.</param>
    public WebSocketHandler(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    /// <summary>
    /// Represents a user's connection details.
    /// </summary>
    public sealed class UserConnection
    {
        public string Username { get; }
        public DateTime ConnectedAt { get; }
        public DateTime LastActivity { get; set; }

        public UserConnection(string username)
        {
            Username = username;
            ConnectedAt = DateTime.UtcNow;
            LastActivity = DateTime.UtcNow;
        }
    }
    /// <summary>
    /// Handles an incoming WebSocket connection.
    /// </summary>
    public async Task HandleConnection(HttpContext context, WebSocket socket)
    {
        var username = context.Request.Query["username"].ToString();
        Console.WriteLine($"Connection attempt from: {username}");

        if (!ValidateUsername(username))
        {
            await CloseConnection(socket, WebSocketCloseStatus.InvalidPayloadData,
                "Invalid username (letters only, max 12 characters)");
            return;
        }

        if (IsUsernameTaken(username))
        {
            await CloseConnection(socket, WebSocketCloseStatus.PolicyViolation,
                "Username already in use");
            return;
        }

        // Create a new scope for this connection
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        try
        {
            // Save/update user in database
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
            {
                user = new User { Username = username, ConnectedTime = DateTime.UtcNow };
                dbContext.Users.Add(user);
            }
            else
            {
                user.ConnectedTime = DateTime.UtcNow;
                user.DisconnectedTime = null;
            }
            await dbContext.SaveChangesAsync();

            // Add connection
            var userConnection = new UserConnection(username);
            if (!_connections.TryAdd(socket, userConnection))
            {
                await CloseConnection(socket, WebSocketCloseStatus.InternalServerError,
                    "Failed to establish connection");
                return;
            }

            Console.WriteLine($"Connection established for {username}");
            await SendWelcomeMessages(dbContext, socket, username);
            await ProcessMessages(dbContext, socket, username);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error handling connection: {ex}");
            await HandleDisconnection(dbContext, username, "encountered an error");
        }
        finally
        {
            await CleanupConnection(socket, username);
        }
    }
    /// <summary>
    /// Processes incoming messages from a WebSocket connection.
    /// </summary>
    public async Task ProcessMessages(ApplicationDbContext dbContext, WebSocket socket, string username)
    {
        var buffer = new byte[1024 * 4];
        var receiveBuffer = new ArraySegment<byte>(buffer);

        try
        {
            while (socket.State == WebSocketState.Open)
            {
                var result = await socket.ReceiveAsync(receiveBuffer, CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await HandleDisconnection(dbContext, username, "disconnected");
                    break;
                }

                if (result.MessageType == WebSocketMessageType.Text)
                {
                    var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    await ProcessSingleMessage(dbContext, socket, username, message);
                }

                UpdateActivityTime(socket);
            }
        }
        catch (WebSocketException)
        {
            await HandleDisconnection(dbContext, username, "disconnected unexpectedly");
        }
    }
    /// <summary>
    /// SINGLE MESSAGE
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="socket"></param>
    /// <param name="username"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    public async Task ProcessSingleMessage(ApplicationDbContext dbContext, WebSocket socket, string username, string message)
    {
        if (message == "!leave")
        {
            await HandleLeaveCommand(dbContext, socket, username);
            return;
        }

        message = message.Trim();
        if (message.Length > 200)
        {
            await SendMessage(socket, "[System] Your message exceeds the 200 character limit");
            return;
        }

        await BroadcastMessage(dbContext, username, message);
    }
    /// <summary>
    /// Broadcasts a chat message to all connected clients and persists it to the database.
    /// </summary>
    /// <param name="dbContext">The database context for accessing and storing chat data.</param>
    /// <param name="sender">The username of the message sender.</param>
    /// <param name="content">The content of the message to be broadcasted.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    /// <remarks>
    /// <para>This method performs two main operations:</para>
    /// <list type="number">
    ///     <item>
    ///         <description>Persists the message to the database by associating it with the sender's user record.</description>
    ///     </item>
    ///     <item>
    ///         <description>Broadcasts the message to all currently connected clients with open WebSocket connections.</description>
    ///     </item>
    /// </list>
    /// <para>The message format for broadcasting is: "{sender}: {content}".</para>
    /// <para>If an error occurs during the process, it will be caught and logged to the console.</para>
    /// </remarks>
    /// <exception cref="DbUpdateException">Thrown when there's an error saving the message to the database.</exception>
    /// <exception cref="WebSocketException">Thrown when there's an error sending the message to connected clients.</exception>
    public async Task BroadcastMessage(ApplicationDbContext dbContext, string sender, string content)
    {
        try
        {
            // Save to database
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Username == sender);
            if (user != null)
            {
                var dbMessage = new Models.ChatMessage
                {
                    Content = content,
                    SentTime = DateTime.UtcNow,
                    UserId = user.Id
                };
                dbContext.ChatMessages.Add(dbMessage);
                await dbContext.SaveChangesAsync();
            }

            // Broadcast to all connected clients
            var message = $"{sender}: {content}";
            var bytes = Encoding.UTF8.GetBytes(message);
            var tasks = new List<Task>();

            foreach (var (socket, _) in _connections)
            {
                if (socket.State == WebSocketState.Open)
                {
                    tasks.Add(socket.SendAsync(
                        new ArraySegment<byte>(bytes),
                        WebSocketMessageType.Text,
                        true,
                        CancellationToken.None));
                }
            }

            await Task.WhenAll(tasks);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error broadcasting message: {ex}");
        }
    }

    public async Task SendWelcomeMessages(ApplicationDbContext dbContext, WebSocket socket, string username)
    {
        try
        {
            // Send message history
            var messages = await dbContext.ChatMessages
                .Include(m => m.User)
                .OrderByDescending(m => m.SentTime)
                .Take(50)
                .ToListAsync();

            foreach (var msg in messages.OrderBy(m => m.SentTime))
            {
                await SendMessage(socket, $"{msg.User.Username}: {msg.Content}");
            }

            // Send welcome message
            await BroadcastSystemMessage($"{username} joined the chat");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending welcome messages: {ex}");
        }
    }

    public async Task BroadcastSystemMessage(string content)
    {
        var message = $"[System] {content}";
        var bytes = Encoding.UTF8.GetBytes(message);
        var tasks = new List<Task>();

        foreach (var (socket, _) in _connections)
        {
            if (socket.State == WebSocketState.Open)
            {
                tasks.Add(socket.SendAsync(
                    new ArraySegment<byte>(bytes),
                    WebSocketMessageType.Text,
                    true,
                    CancellationToken.None));
            }
        }

        await Task.WhenAll(tasks);
    }
    /// <summary>
    /// Handles the leave command for a user by broadcasting a system message about their departure
    /// and closing their WebSocket connection with a normal closure status.
    /// </summary>
    /// <param name="dbContext">The database context for accessing application data</param>
    /// <param name="socket">The WebSocket connection of the user who is leaving</param>
    /// <param name="username">The username of the user who is leaving</param>
    /// <returns>A Task representing the asynchronous operation</returns>
    /// <remarks>
    /// This method performs two main actions:
    /// 1. Broadcasts a system message to all connected clients notifying them that the user has left
    /// 2. Closes the user's WebSocket connection with a normal closure status and "User left" reason
    /// </remarks>
    public async Task HandleLeaveCommand(ApplicationDbContext dbContext, WebSocket socket, string username)
    {
        await BroadcastSystemMessage($"{username} has left the chat");
        await CloseConnection(socket, WebSocketCloseStatus.NormalClosure, "User left");
    }

    public async Task HandleDisconnection(ApplicationDbContext dbContext, string username, string reason)
    {
        try
        {
            // Update user in database
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user != null)
            {
                user.DisconnectedTime = DateTime.UtcNow;
                await dbContext.SaveChangesAsync();
            }

            await BroadcastSystemMessage($"{username} has {reason}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error handling disconnection: {ex}");
        }
    }

    public async Task CleanupConnection(WebSocket socket, string username)
    {
        _connections.TryRemove(socket, out _);

        try
        {
            if (socket.State == WebSocketState.Open)
            {
                await socket.CloseAsync(WebSocketCloseStatus.NormalClosure,
                    "Connection closed", CancellationToken.None);
            }
        }
        catch
        {
            // Ignore close errors
        }
        finally
        {
            socket.Dispose();
        }
    }

    public async Task CloseConnection(WebSocket socket, WebSocketCloseStatus status, string reason)
    {
        try
        {
            if (socket.State == WebSocketState.Open)
            {
                await socket.CloseAsync(status, reason, CancellationToken.None);
            }
        }
        finally
        {
            socket.Dispose();
        }
    }

    public async Task SendMessage(WebSocket socket, string message)
    {
        if (socket.State == WebSocketState.Open)
        {
            var bytes = Encoding.UTF8.GetBytes(message);
            await socket.SendAsync(
                new ArraySegment<byte>(bytes),
                WebSocketMessageType.Text,
                true,
                CancellationToken.None);
        }
    }

    public void UpdateActivityTime(WebSocket socket)
    {
        if (_connections.TryGetValue(socket, out var connection))
        {
            connection.LastActivity = DateTime.UtcNow;
        }
    }
    /// <summary>
    /// Validates if a username meets the criteria (letters only, max 12 characters).
    /// </summary>
    public bool ValidateUsername(string username)
    {
        // Should return true for "MaximumLength" (12 chars)
        return !string.IsNullOrEmpty(username) &&
               username.Length <= 12 &&
               username.All(char.IsLetter);
    }
    /// <summary>
    /// Checks if a username is already in use.
    /// </summary>
    public bool IsUsernameTaken(string username)
    {
        return _connections.Values.Any(c =>
            string.Equals(c.Username, username, StringComparison.OrdinalIgnoreCase));
    }

    public int GetActiveConnectionsCount() => _connections.Count;

    public IEnumerable<string> GetConnectedUsernames()
    {
        return _connections.Values
            .OrderBy(c => c.ConnectedAt)
            .Select(c => $"{c.Username} (since {c.ConnectedAt:HH:mm})");
    }
}