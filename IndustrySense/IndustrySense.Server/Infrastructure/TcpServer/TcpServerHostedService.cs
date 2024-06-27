namespace IndustrySense.Server.Infrastructure.TcpServer
{
    public class TcpServerHostedService : BackgroundService
    {
        private readonly ITcpServer _tcpServer;

        public TcpServerHostedService(ITcpServer tcpServer)
        {
            _tcpServer = tcpServer;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _tcpServer.Start();
            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken stoppingToken)
        {
            _tcpServer.Stop();
            return base.StopAsync(stoppingToken);
        }
    }
}
