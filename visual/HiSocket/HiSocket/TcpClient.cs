//****************************************************************************
// Description:
// Author: hiramtan@qq.com
//***************************************************************************

using System;
using System.IO;
using System.Net.Sockets;

namespace HiSocket.Tcp
{
    class TcpClient : ISocket
    {
        public int TimeOut
        {
            private get { return timeOut; }
            set { timeOut = value; }
        }

        public int ReceiveBufferSize
        {
            private get { return receiveBufferSize; }
            set
            {
                receiveBufferSize = value;
                buffer = new byte[ReceiveBufferSize];
            }
        }

        public Action<SocketState> StateEvent { get; set; }
        public bool IsConnected { get { return tcp != null && tcp.Client != null && tcp.Connected; } }

        private string ip;
        private int port;
        private IProto iProto;
        private System.Net.Sockets.TcpClient tcp;
        private int receiveBufferSize = 1024 * 128;//128k
        private byte[] buffer;
        private int timeOut = 5000;//5s:收发超时时间
        MemoryStream msSend = new MemoryStream();
        MemoryStream msReceive = new MemoryStream();
        public TcpClient(IProto iProto)
        {
            buffer = new byte[ReceiveBufferSize];
            this.iProto = iProto;
            tcp = new System.Net.Sockets.TcpClient();
            tcp.NoDelay = true;
            tcp.SendTimeout = tcp.ReceiveTimeout = TimeOut;
        }

        public void Connect(string ip, int port)
        {
            ChangeState(SocketState.Connecting);
            if (IsConnected)
            {
                ChangeState(SocketState.Connected);
                return;
            }
            try
            {
                this.tcp.BeginConnect(ip, port, (delegate (IAsyncResult ar)
                {
                    try
                    {
                        System.Net.Sockets.TcpClient tcp = ar.AsyncState as System.Net.Sockets.TcpClient;
                        tcp.EndConnect(ar);
                        if (tcp.Connected)
                        {
                            ChangeState(SocketState.Connected);
                            tcp.Client.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, Receive, tcp);
                        }
                        else ChangeState(SocketState.DisConnected);
                    }
                    catch (Exception e)
                    {
                        ChangeState(SocketState.DisConnected);
                        throw new Exception(e.ToString());
                    }
                }), this.tcp);
            }
            catch (Exception e)
            {
                ChangeState(SocketState.DisConnected);
                throw new Exception(e.ToString());
            }
        }

        public void Send(byte[] bytes)
        {
            if (!IsConnected)
            {
                ChangeState(SocketState.DisConnected);
                throw new Exception("receive failed");
            }
            try
            {
                msSend.Seek(0, SeekOrigin.End);
                msSend.Write(bytes, 0, bytes.Length);
                msSend.Seek(0, SeekOrigin.Begin);
                iProto.Pack(msSend);
                var toSend = msReceive.GetBuffer();
                tcp.Client.BeginSend(toSend, 0, toSend.Length, SocketFlags.None, delegate (IAsyncResult ar)
                {
                    try
                    {
                        System.Net.Sockets.TcpClient tcp = ar.AsyncState as System.Net.Sockets.TcpClient;
                        tcp.Client.EndSend(ar);
                    }
                    catch (Exception e)
                    {
                        ChangeState(SocketState.DisConnected);
                        throw new Exception(e.ToString());
                    }
                }, tcp);
            }
            catch (Exception e)
            {
                ChangeState(SocketState.DisConnected);
                throw new Exception(e.ToString());
            }
        }

        void Receive(IAsyncResult ar)
        {
            if (!IsConnected)
            {
                ChangeState(SocketState.DisConnected);
                throw new Exception("receive failed");
            }
            try
            {
                System.Net.Sockets.TcpClient tcp = ar as System.Net.Sockets.TcpClient;
                int length = tcp.Client.EndReceive(ar);
                if (length > 0)
                {
                    msReceive.Write(buffer, 0, length);
                    iProto.Unpack(msReceive);
                }
                tcp.Client.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, Receive, tcp);
            }
            catch (Exception e)
            {
                ChangeState(SocketState.DisConnected);
                throw new Exception(e.ToString());
            }
        }

        public void DisConnect()
        {
            if (IsConnected)
            {
                //tcp.Client.Shutdown(SocketShutdown.Both);
                tcp.Close();//close already contain shutdown
                tcp = null;
            }
            ChangeState(SocketState.DisConnected);
            //StateEvent = null;
        }
        public long Ping()
        {
            System.Net.NetworkInformation.Ping tempPing = new System.Net.NetworkInformation.Ping();
            System.Net.NetworkInformation.PingReply temPingReply = tempPing.Send(ip);
            return temPingReply.RoundtripTime;
        }


        private void ChangeState(SocketState state)
        {
            if (StateEvent != null)
            {
                StateEvent(state);
            }
        }
    }
}