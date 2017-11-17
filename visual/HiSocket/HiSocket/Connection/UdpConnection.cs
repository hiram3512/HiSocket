//****************************************************************************
// Description:
// Author: hiramtan@qq.com
//***************************************************************************
using System;
using System.Net.Sockets;
using HiSocket.Msg;

namespace HiSocket
{
    public class UdpConnection : Connection
    {
        private UdpClient _client;
        public override bool IsConnected => _client != null && _client.Client != null && _client.Client.Connected;
        public UdpConnection(IPackage iPackage):base(iPackage)
        {
            _client = new UdpClient();
            _client.Client.NoDelay = true;
            _client.Client.SendTimeout = _client.Client.ReceiveTimeout = TimeOut;
        }

        public override void Connect(string ip, int port)
        {
            ChangeState(SocketState.Connecting);
            if (IsConnected)
            {
                ChangeState(SocketState.Connected);
                Console.WriteLine("already connected");
                return;
            }
            try
            {
                _client.Client.BeginConnect(ip, port, (x) =>
                {
                    var udp = x.AsyncState as UdpClient;
                    udp.Client.EndConnect(x);
                    if (udp.Client.Connected)
                    {
                        ChangeState(SocketState.Connected);
                        udp.Client.BeginReceive(_receiveBuffer, 0, _receiveBuffer.Length, SocketFlags.None, Receive, udp);
                    }
                    else
                    {
                        ChangeState(SocketState.DisConnected);
                    }
                }, _client);
            }
            catch (Exception e)
            {
                ChangeState(SocketState.DisConnected);
                throw new Exception(e.ToString());
            }
        }
        public override void DisConnect()
        {
            if (IsConnected)
            {
                _client.Client.Shutdown(SocketShutdown.Both);
                _client.Close();
                _client = null;
            }
            ChangeState(SocketState.DisConnected);
            StateChangeHandler = null;
        }
        private void Receive(IAsyncResult ar)
        {

        }

        public override void Send(byte[] bytes)
        {
            if (!IsConnected)
            {
                ChangeState(SocketState.DisConnected);
                throw new Exception("from send: disconnected");
            }
            _iByteArraySend.Clear();
            _iByteArraySend.Write(bytes, bytes.Length);
            try
            {
                _iPackage.Pack(_iByteArraySend);
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
            var toSend = _iByteArraySend.Read(_iByteArraySend.Length);
            try
            {
                _client.Client.BeginSend(toSend, 0, toSend.Length, SocketFlags.None, delegate (IAsyncResult ar)
                {
                    var udp = ar.AsyncState as UdpClient;
                    udp.Client.EndSend(ar);
                }, _client);
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
        }
    }
}
