//*********************************************************************
// Description:
// Author: hiramtan@live.com
//*********************************************************************

using System;
using System.Net.Sockets;

namespace HiTCP
{
    public class SocketClient : ISocket, IDisposable
    {
        private int bufferSize = 1024;
        private string ip;
        private int port;
        public byte[] buffer;
        private TcpClient client;
        private int timeOut = 5000;//5s
        public bool IsConnected { get { return client.Client != null && client.Connected; } }

        public SocketClient()
        {
            client = new TcpClient();
            client.SendTimeout = client.ReceiveTimeout = timeOut;
            buffer = new byte[bufferSize];
        }

        public void Connect(string paramIp, int paramPort)
        {
            ip = paramIp;
            port = paramPort;

        }

        public void Send(byte[] param)
        {
            throw new NotImplementedException();
        }

        public void Receive()
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}