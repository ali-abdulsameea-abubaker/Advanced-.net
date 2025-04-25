using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealTimeChatApp.Hubs;
using RealTimeChatApp.Models;

namespace RealTimeChatApp.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _dbContext;
    private readonly WebSocketHandler _webSocketHandler;

    public HomeController(ApplicationDbContext dbContext, WebSocketHandler webSocketHandler)
    {
        _dbContext = dbContext;
        _webSocketHandler = webSocketHandler;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [HttpGet("ws-url")]
    public IActionResult GetWebSocketUrl()
    {
        var isHttps = Request.IsHttps;
        var host = Request.Host;
        var wsScheme = isHttps ? "wss" : "ws";
        return Json(new { url = $"{wsScheme}://{host}/ws" });
    }

    [HttpGet("chat/history")]
    public async Task<IActionResult> GetMessageHistory(int before = 0)
    {
        try
        {
            // Create the base query with filtering
            IQueryable<ChatMessage> query = _dbContext.ChatMessages
                .Include(m => m.User);

            if (before > 0)
            {
                query = query.Where(m => m.Id < before);
            }

            // Now apply ordering, take, and select
            var messages = await query
                .OrderByDescending(m => m.SentTime)
                .Take(20)
                .Select(m => new
                {
                    m.Id,
                    m.Content,
                    Sender = m.User.Username,
                    SentAt = m.SentTime,
                    IsSystemMessage = false
                })
                .ToListAsync();

            return Ok(messages);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error retrieving message history: {ex.Message}");
        }
    }

    [HttpGet("active-connections")]
    public IActionResult GetActiveConnections()
    {
        return Json(new
        {
            count = _webSocketHandler.GetActiveConnectionsCount(),
            users = _webSocketHandler.GetConnectedUsernames()
        });
    }
}