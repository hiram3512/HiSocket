//****************************************************************************
// Description:
// Author: hiramtan@qq.com
//***************************************************************************
using System;

namespace HiSocket.Udp
{
    public class UdpClient : ISocket
    {
        public int TimeOut { get; set; }
        public int ReceiveBufferSize { get; set; }
        public Action<SocketState> StateChangeHandler { get; set; }
        public bool IsConnected { get; }
        public void Connect(string ip, int port)
        {
            throw new NotImplementedException();
        }

        public void DisConnect()
        {
            throw new NotImplementedException();
        }

        public void Send(byte[] bytes)
        {
            throw new NotImplementedException();
        }

        public long Ping()
        {
            throw new NotImplementedException();
        }
    }
}
