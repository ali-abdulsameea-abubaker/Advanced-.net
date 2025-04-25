using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Net.WebSockets;
using System.Text;

namespace RealTimeChatApp.Utilities;

/// <summary>
/// Static class that manages WebSocket connections and message broadcasting for a real-time chat application.
/// </summary>
/// <remarks>
/// This handler maintains active WebSocket connections and provides functionality for broadcasting messages
/// to all connected clients. It includes methods for connection handling, message broadcasting, and test support.
/// </remarks>
public static class ChatHandler
{
    private static readonly List<WebSocket> _sockets = new();
    private static readonly Dictionary<WebSocket, string> _users = new();

    /// <summary>
    /// Handles a new WebSocket connection for a chat participant.
    /// </summary>
    /// <param name="context">The HTTP context containing the WebSocket request.</param>
    /// <param name="socket">The WebSocket connection to handle.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    /// <remarks>
    /// This method validates the username, manages the connection lifecycle, and handles message receiving/broadcasting.
    /// Usernames must be 1-12 characters long and contain no numbers.
    /// </remarks>
    public static async Task HandleConnection(HttpContext context, WebSocket socket)
    {
        var username = context.Request.Query["username"].ToString();

        // Enhanced username validation
        if (string.IsNullOrEmpty(username) || username.Length > 12 || username.Any(char.IsDigit))
        {
            await socket.CloseAsync(WebSocketCloseStatus.InvalidPayloadData,
                "Invalid username (must be 1-12 letters, no numbers)",
                CancellationToken.None);
            return;
        }

        // Add to connections
        _sockets.Add(socket);
        _users[socket] = username;

        await Broadcast($"{username} joined the chat");

        try
        {
            var buffer = new byte[1024];
            while (socket.State == WebSocketState.Open)
            {
                var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Text)
                {
                    var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    await Broadcast($"{username}: {message}");
                }
                else if (result.MessageType == WebSocketMessageType.Close)
                {
                    break;
                }
            }
        }
        finally
        {
            _sockets.Remove(socket);
            _users.Remove(socket);
            await Broadcast($"{username} left the chat");
            await socket.CloseAsync(WebSocketCloseStatus.NormalClosure,
                "Closing", CancellationToken.None);
        }
    }

    /// <summary>
    /// Broadcasts a message to all connected WebSocket clients.
    /// </summary>
    /// <param name="message">The message to broadcast.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    /// <remarks>
    /// This method sends the specified message to all currently connected clients
    /// whose WebSocket state is Open. The message is encoded as UTF-8 text.
    /// </remarks>
    internal static async Task Broadcast(string message)
    {
        var bytes = Encoding.UTF8.GetBytes(message);
        var tasks = new List<Task>();

        foreach (var socket in _sockets.Where(s => s.State == WebSocketState.Open))
        {
            tasks.Add(socket.SendAsync(new ArraySegment<byte>(bytes),
                WebSocketMessageType.Text,
                true,
                CancellationToken.None));
        }

        await Task.WhenAll(tasks);
    }

    /// <summary>
    /// Adds a WebSocket connection to the handler for testing purposes.
    /// </summary>
    /// <param name="socket">The WebSocket connection to add.</param>
    /// <param name="username">The username associated with the connection.</param>
    /// <remarks>
    /// This test support method allows simulating connected clients in unit tests.
    /// </remarks>
    public static void AddSocketForTesting(WebSocket socket, string username)
    {
        _sockets.Add(socket);
        _users[socket] = username;
    }

    /// <summary>
    /// Clears all WebSocket connections from the handler for testing purposes.
    /// </summary>
    /// <remarks>
    /// This test support method resets the handler's state between unit tests.
    /// </remarks>
    public static void ClearSocketsForTesting()
    {
        _sockets.Clear();
        _users.Clear();
    }

    /// <summary>
    /// Wrapper method for testing the Broadcast functionality.
    /// </summary>
    /// <param name="message">The message to broadcast.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    /// <remarks>
    /// This test support method provides access to the internal Broadcast method for testing purposes.
    /// </remarks>
    public static async Task TestBroadcast(string message)
    {
        await Broadcast(message);
    }

    /// <summary>
    /// Gets the current number of active connections for testing purposes.
    /// </summary>
    /// <returns>The count of active WebSocket connections.</returns>
    /// <remarks>
    /// This test support method allows verifying connection counts in unit tests.
    /// </remarks>
    public static int GetConnectionCountForTesting() => _sockets.Count;
}