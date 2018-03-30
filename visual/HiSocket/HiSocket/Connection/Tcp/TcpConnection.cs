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
        private ByteBlockBuffer _sendBuffer = new ByteBlockBuffer();
        private ByteBlockBuffer _receiveBuffer = new ByteBlockBuffer();
        private ByteArray _sendArray = new ByteArray();
        private ByteArray _receiveArray = new ByteArray();
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
                        throw new Exception("socket connect failed");
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
                int count = _sendBuffer.Reader.GetHowManyCountCanReadInThisBlock();
                if (count > 0)
                {
                    lock (_sendBuffer)
                    {
                        try
                        {
                            var length = _socket.Send(_sendBuffer.Reader.Node.Value, _sendBuffer.Reader.Position, count, SocketFlags.None);
                            _sendBuffer.Reader.MovePosition(length);
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
                    lock (_receiveBuffer)
                    {
                        try
                        {
                            var count = _receiveBuffer.Writer.GetHowManyCountCanWriteInThisBlock();
                            var length = _socket.Receive(_receiveBuffer.Writer.Node.Value, _receiveBuffer.Writer.Position, count, SocketFlags.None);
                            _receiveBuffer.Writer.MovePosition(length);
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
            if (_sendQueue.Count == 0)
                return;
            lock (_sendBuffer)
            {
                try
                {
                    _iPackage.Pack(_sendQueue, _sendArray);
                    _sendBuffer.WriteAllBytes(_sendArray.Read(_sendArray.Length));
                }
                catch (Exception e)
                {
                    throw new Exception("pack error: " + e);
                }
            }
        }
        void UnPack()
        {
            lock (_receiveBuffer)
            {
                var bytes = _receiveBuffer.ReadAllBytes();
                if (bytes.Length == 0)
                    return;
                try
                {
                    _receiveArray.Write(bytes);
                    _iPackage.Unpack(_receiveArray, _receiveQueue);
                }
                catch (Exception e)
                {
                    throw new Exception(e.ToString());
                }
            }
        }
    }
}