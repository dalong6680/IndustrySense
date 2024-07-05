using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using IndustrySense.Server.Common.Executor;
using IndustrySense.Server.Common.TcpServer;
using Microsoft.Extensions.Hosting;

namespace IndustrySense.Server.Application.Services
{
    public class TcpMessageHandlerService : BackgroundService
    {
        private readonly ITcpServer _tcpServer;

        public TcpMessageHandlerService(ITcpServer tcpServer)
        {
            _tcpServer = tcpServer;
            _tcpServer.MessageReceived += OnMessageReceived;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Run(
                () =>
                {
                    _tcpServer.Start();

                    // 监听停止信号并停止TCP服务器
                    stoppingToken.Register(() => _tcpServer.Stop());
                },
                stoppingToken
            );
        }

        private void OnMessageReceived(object? sender, string message)
        {
            // 示例处理：假设处理是异步的
            TcpClient? client = sender as TcpClient;
            var remoteEndPoint = client?.Client.RemoteEndPoint as IPEndPoint;
            var ip = remoteEndPoint?.Address.ToString();






            ProcessMessageAsync(message).GetAwaiter().GetResult();
        }

        private Task ProcessMessageAsync(string message)
        {
            string param = message;
            string lua =
                @"
                    local intValue = tonumber(data, 16)
                    if intValue > 32767 then
                        intValue = intValue - 65536
                    end
                    local temperature = intValue * 0.0625
                    local formattedTemperature = string.format(""%.2f"", temperature)
                    return formattedTemperature
                ";
            LuaExecutor executor = new LuaExecutor();
            var res = executor.ExecuteScript(lua, message)?[0].ToString();

            Console.WriteLine($"Processed message: {res}");

            // 假设消息处理需要异步操作
            return Task.CompletedTask;
        }
    }
}
