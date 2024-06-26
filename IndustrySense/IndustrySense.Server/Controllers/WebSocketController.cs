using System.Diagnostics;
using System.Net.WebSockets;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace IndustrySense.Server.Services
{
    [ApiController]
    public class WebSocketController : ControllerBase
    {
        [HttpGet("/ws")]
        public async Task Get()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                await Echo(webSocket);
            }
            else
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
        }

        private async Task Echo(WebSocket webSocket)
        {
            var random = new Random();
            var buffer = new byte[1024 * 4];
            while (webSocket.State == WebSocketState.Open)
            {
                var temperature = random.Next(-10, 35);
                var message = Encoding.UTF8.GetBytes(temperature.ToString());
                Task<WebSocketReceiveResult> receiveTask = webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                try
                {
                    await webSocket.SendAsync(
                        new ArraySegment<byte>(message),
                        WebSocketMessageType.Text,
                        true,
                        CancellationToken.None
                    );
                    Console.WriteLine($"Sent temperature: {temperature}");
                    webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                }
                catch (WebSocketException ex)
                {
                    Console.WriteLine($"WebSocket exception: {ex.Message}");
                    break;
                }
                await Task.Delay(1000); // 每秒发送一次数据
            }
        }
    }
}
