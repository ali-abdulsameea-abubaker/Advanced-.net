using Microsoft.EntityFrameworkCore;
using RealTimeChatApp.Hubs;
using RealTimeChatApp.Models;
/// <summary>
///  <Authorship>I, Ali Abdulsameea, 000857347 certify that this material is my original work.
/// No other person's work has been used without due acknowledgement.</Authorship>
///
/// 
/// <author>Ali Abubaker - 000857347</author>
/// </summary>
/// 
/// <summary>
/// The main entry point for the Real-Time Chat Application.
/// Configures services and middleware for handling WebSockets and HTTP requests.
/// </summary>
var builder = WebApplication.CreateBuilder(args);

/// <summary>
/// Adds services to the container, including MVC controllers and the database context.
/// </summary>
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

/// <summary>
/// Registers WebSocketHandler as a singleton service to manage WebSocket connections.
/// </summary>
builder.Services.AddSingleton<WebSocketHandler>();

var app = builder.Build();

/// <summary>
/// Configures the HTTP request pipeline.
/// </summary>
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStaticFiles();
app.UseRouting();

/// <summary>
/// Configures WebSocket middleware with keep-alive interval and buffer size.
/// </summary>
app.UseWebSockets(new WebSocketOptions
{
    KeepAliveInterval = TimeSpan.FromSeconds(120),
    ReceiveBufferSize = 4 * 1024
});

/// <summary>
/// Handles WebSocket requests at the "/ws" endpoint.
/// If the request is not a WebSocket request, returns a 400 Bad Request response.
/// </summary>
app.Use(async (context, next) =>
{
    if (context.Request.Path == "/ws")
    {
        if (context.WebSockets.IsWebSocketRequest)
        {
            using var webSocket = await context.WebSockets.AcceptWebSocketAsync();
            var handler = context.RequestServices.GetRequiredService<WebSocketHandler>();
            await handler.HandleConnection(context, webSocket);
        }
        else
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
        }
    }
    else
    {
        await next();
    }
});

/// <summary>
/// Maps the default controller route.
/// </summary>
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

/// <summary>
/// Runs the application.
/// </summary>
app.Run();
