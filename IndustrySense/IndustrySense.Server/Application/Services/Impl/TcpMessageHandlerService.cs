using System;
using System.Threading;
using System.Threading.Tasks;
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
            ProcessMessageAsync(message).GetAwaiter().GetResult();
        }

        private Task ProcessMessageAsync(string message)
        {
            // 这里实现具体的消息处理逻辑
            // 示例：简单打印消息



            Console.WriteLine($"Processed message: {message}");

            // 假设消息处理需要异步操作
            return Task.CompletedTask;
        }
    }
}
