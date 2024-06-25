using System.Net.WebSockets;
using System.Text;
using Microsoft.AspNetCore.WebSockets;

namespace IndustrySense.Server.Services
{
    public class WebSocketMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<WebSocketMiddleware> _logger;

        public WebSocketMiddleware(RequestDelegate next, ILogger<WebSocketMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path == "/ws")
            {
                if (context.WebSockets.IsWebSocketRequest)
                {
                    using var webSocket = await context.WebSockets.AcceptWebSocketAsync();
                    await HandleWebSocketAsync(webSocket);
                }
                else
                {
                    context.Response.StatusCode = 400;
                }
            }
            else
            {
                await _next(context);
            }
        }

        private async Task HandleWebSocketAsync(WebSocket webSocket)
        {
            var random = new Random();
            while (webSocket.State == WebSocketState.Open)
            {
                var temperature = random.Next(-10, 35);
                var message = Encoding.UTF8.GetBytes(temperature.ToString());
                await webSocket.SendAsync(
                    new ArraySegment<byte>(message),
                    WebSocketMessageType.Text,
                    true,
                    CancellationToken.None
                );
                _logger.LogInformation($"Sent temperature: {temperature}");
                await Task.Delay(1000); // 每秒发送一次数据
            }
        }
    }
}
