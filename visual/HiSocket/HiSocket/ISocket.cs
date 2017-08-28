namespace HiSocket
{
    public interface ISocket
    {
        int Buffer { set; }

        bool IsConnected { get; }
        void Connect(string ip, int port);
        void DisConnect();
        void Send(byte[] bytes);
        long Ping();
    }

}
