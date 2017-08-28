//****************************************************************************
// Description:
// Author: hiramtan@qq.com
//***************************************************************************

using System;
using System.Net.Sockets;

namespace HiSocket.Tcp
{
    class TcpClient : ISocket
    {
        public int Buffer
        {
            private get { return m_Buffer; }
            set { m_Buffer = value; }
        }

        public bool IsConnected { get; private set; }




        private string ip;
        private int port;
        private IProto iProto;
        private System.Net.Sockets.TcpClient tcp;
        private int m_Buffer = 1024 * 128;//128k
        private int timeOut = 5000;//5s:收发超时时间





        public TcpClient(IProto iProto, AddressFamily addressFamily = AddressFamily.InterNetwork)
        {
            this.iProto = iProto;
            tcp = new System.Net.Sockets.TcpClient(addressFamily);
            tcp.NoDelay = true;
            tcp.SendTimeout = tcp.ReceiveTimeout = timeOut;
        }


        public void Connect(string ip, int port)
        {
            if (IsConnected)
                return;

            bool isSuccess = false;
            try
            {
                this.tcp.BeginConnect(ip, port, (delegate (IAsyncResult ar)
                {
                    System.Net.Sockets.TcpClient tcp = ar.AsyncState as System.Net.Sockets.TcpClient;
                    tcp.EndConnect(ar);
                    isSuccess = ar.IsCompleted;

                }), this.tcp);
            }
            catch (Exception e)
            {

                throw new Exception(e.ToString());
            }



        }

        public void DisConnect()
        {
            throw new System.NotImplementedException();
        }

        public void Send(byte[] bytes)
        {
            throw new System.NotImplementedException();
        }

        public long Ping()
        {
            throw new System.NotImplementedException();
        }

    }
}
