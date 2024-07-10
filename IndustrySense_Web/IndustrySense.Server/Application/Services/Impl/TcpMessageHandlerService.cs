using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using IndustrySense.Server.Application.Dto;
using IndustrySense.Server.Common.Executor;
using IndustrySense.Server.Common.TcpServer;
using IndustrySense.Server.Infrastructure.Data.Entity;
using Microsoft.Extensions.Hosting;

namespace IndustrySense.Server.Application.Services
{
    public class TcpMessageHandlerService : BackgroundService
    {
        private readonly IRecordService _recordService;
        private readonly IParsingRuleService _parsingRuleService;
        private readonly IDeviceService _deviceService;
        private readonly ITcpServer _tcpServer;

        public TcpMessageHandlerService(
            ITcpServer tcpServer,
            IDeviceService deviceService,
            IParsingRuleService parsingRuleService,
            IRecordService recordService
        )
        {
            _recordService = recordService;
            _parsingRuleService = parsingRuleService;
            _deviceService = deviceService;
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
            TcpClient? client = sender as TcpClient;
            var remoteEndPoint = client?.Client.RemoteEndPoint as IPEndPoint;
            var ip = remoteEndPoint?.Address.ToString();
            if (string.IsNullOrEmpty(ip))
            {
                return;
            }
            var device = _deviceService.GetDeviceByIpAddress(ip);
            if (device == null)
            {
                _deviceService.AddDevice(
                    new Device() { DeviceIpAddress = ip, DeviceName = "未命名设备" }
                );
                device = _deviceService.GetDeviceByIpAddress(ip);
            }
            string content = message;

            _recordService.AddRecord(
                new Record()
                {
                    DeviceId = device.DeviceId,
                    Timestamp = DateTime.Now,
                    Content = content
                }
            );
        }
    }
}
