using System;

namespace HiSocket
{
    public interface ISocket
    {
        int TimeOut { set; }
        int ReceiveBufferSize { set; }

        Action<SocketState> StateEvent { get; set; }

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
