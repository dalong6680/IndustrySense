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
            if (device!.ParsingRuleId != 0)
            {
                string script = _parsingRuleService.GetParsingRuleScriptById(device.ParsingRuleId)!;
                content = ProcessMessage(message, script)!;
            }
            _recordService.AddRecord(
                new Record()
                {
                    DeviceId = device.DeviceId,
                    Timestamp = DateTime.Now,
                    Content = content
                }
            );
        }

        private string? ProcessMessage(string message, string script)
        {
            //string lua =
            //    @"
            //        local intValue = tonumber(data, 16)
            //        if intValue > 32767 then
            //            intValue = intValue - 65536
            //        end
            //        local temperature = intValue * 0.0625
            //        local formattedTemperature = string.format(""%.2f"", temperature)
            //        return formattedTemperature
            //    ";
            LuaExecutor executor = new LuaExecutor();
            var res = executor.ExecuteScript(script, message);
            var processedRes = res?[0]?.ToString();

            Console.WriteLine($"Processed message: {processedRes}");

            return processedRes;
        }
    }
}
