//****************************************************************************
// Description: socket of tcp protocol connection
// Author: hiramtan@live.com
//****************************************************************************
using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace HiSocket
{
    public class TcpConnection : Connection, ITcp
    {
        private ByteBlockArray sendArray = new ByteBlockArray();
        private ByteBlockArray receiveArray = new ByteBlockArray();
        private readonly IPackage _iPackage;
        private int _timeOut = 5000;
        public int TimeOut
        {
            get { return _timeOut; }
            set { _timeOut = value; }
        }

        public bool IsConnected
        {
            get { return _socket != null && _socket.Connected; }
        }
        public event Action<SocketState> StateChangeEvent;
        public TcpConnection(IPackage iPackage)
        {
            _iPackage = iPackage;
        }

        public override void Connect(string ip, int port)
        {
            if (IsConnected)
            {
                Debug.LogWarning("already connected");
                return;
            }
            ChangeState(SocketState.Connecting);
            var address = Dns.GetHostAddresses(ip)[0];
            IPEndPoint iep = new IPEndPoint(address, port);
            _socket = new Socket(address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            _socket.NoDelay = true;
            _socket.SendTimeout = _socket.ReceiveTimeout = TimeOut;
            try
            {
                _socket.BeginConnect(iep, delegate (IAsyncResult ar)
                {
                    var tcp = ar.AsyncState as Socket;
                    if (tcp != null && tcp.Connected)
                    {
                        tcp.EndConnect(ar);
                        ChangeState(SocketState.Connected);
                        InitThread();
                    }
                    else
                    {
                        ChangeState(SocketState.DisConnected);
                        throw new Exception("socket is null or isconnected is false");
                    }
                }, _socket);
            }
            catch (Exception e)
            {
                ChangeState(SocketState.DisConnected);
                throw new Exception(e.ToString());
            }
        }

        protected override void Send()
        {
            while (_isSendThreadOn)
            {
                if (!IsConnected)
                {
                    throw new Exception("from send: disconnected");
                }
                int count = sendArray.GetHowManyCountCanReadInThisBlock();
                if (count > 0)
                {
                    lock (sendArray)
                    {
                        try
                        {
                            var length = _socket.Send(sendArray.Reader.Node.Value, sendArray.Reader.Position, count, SocketFlags.None);
                            sendArray.Read(length);
                        }
                        catch (Exception e)
                        {
                            throw new Exception(e.ToString());
                        }
                    }
                }
            }
        }

        protected override void Receive()
        {
            while (_isReceiveThreadOn)
            {
                if (!IsConnected)
                {
                    throw new Exception("from receive: disconnected");
                }
                if (_socket.Available > 0)
                {
                    lock (receiveArray)
                    {
                        try
                        {
                            var count = receiveArray.GetHowManyCountCanWriteInThisBlock();
                            var length = _socket.Receive(receiveArray.Writer.Node.Value, receiveArray.Writer.Position, count, SocketFlags.None);
                            receiveArray.Write(length);
                        }
                        catch (Exception e)
                        {
                            throw new Exception(e.ToString());
                        }
                    }
                }
            }
        }
        void ChangeState(SocketState state)
        {
            if (StateChangeEvent != null)
                StateChangeEvent(state);
        }

        public override void Run()
        {
            base.Run();
            Pack();
            UnPack();
        }

        void Pack()
        {
            try
            {
                lock (_sendQueue)
                {
                    _iPackage.Pack(_sendQueue, _iByteArraySend);
                }
            }
            catch (Exception e)
            {
                throw new Exception("pack error: " + e);
            }
        }
        void UnPack()
        {
            try
            {
                lock (_receiveQueue)
                {
                    _iPackage.Unpack(_iByteArrayReceive, _receiveQueue);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
        }
    }
}