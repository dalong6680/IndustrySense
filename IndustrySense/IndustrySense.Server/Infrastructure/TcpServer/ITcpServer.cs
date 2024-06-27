namespace IndustrySense.Server.Infrastructure.TcpServer
{
    public interface ITcpServer
    {
        void Start();
        void Stop();
        event EventHandler<string>? MessageReceived;
    }
}
