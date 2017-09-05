//****************************************************************************
// Description:
// Author: hiramtan@qq.com
//***************************************************************************
using System;
using HiSocket.Msg;

namespace HiSocket.Udp
{
    public class UdpClient : ISocket
    {
        public int TimeOut { get; set; }
        public int ReceiveBufferSize { get; set; }
        public Action<SocketState> StateEvent { get; set; }
        public bool IsConnected { get; }

        private IPackage _iPackage;
        private System.Net.Sockets.UdpClient _client;
        public UdpClient(IPackage iPackage)
        {
            _iPackage = iPackage;
        }

        public void Connect(string ip, int port)
        {
            _client.Client.BeginConnect(ip, port, (x) =>
            {
                var client = x.AsyncState as System.Net.Sockets.UdpClient;
                client.Client.EndConnect(x);
                //if(client)
            }, _client);


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
