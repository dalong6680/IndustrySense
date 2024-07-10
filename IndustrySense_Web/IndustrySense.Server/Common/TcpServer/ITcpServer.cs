namespace IndustrySense.Server.Common.TcpServer
{
    public interface ITcpServer
    {
        int ClientCount { get; }

        void Start();
        void Stop();
        event EventHandler<string>? MessageReceived;
    }
}
