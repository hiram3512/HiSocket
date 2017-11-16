using System;

namespace HiSocket
{
    public interface ISocket
    {
        int TimeOut { get; set; }
        int ReceiveBufferSize { get; set; }

        Action<SocketState> StateChangeHandler { set; }

        bool IsConnected { get; }

        void Connect(string ip, int port);
        void DisConnect();
        void Send(byte[] bytes);
        long Ping();
    }
    public enum SocketState
    {
        Connected,
        DisConnected,
        Connecting,
    }
}
