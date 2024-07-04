namespace IndustrySense.Server.Common.TcpServer
{
    public interface ITcpServer
    {
        void Start();
        void Stop();
        event EventHandler<string>? MessageReceived;
    }
}
