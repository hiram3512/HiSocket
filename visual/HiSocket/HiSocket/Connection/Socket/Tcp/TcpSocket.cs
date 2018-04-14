/***************************************************************
 * Description:
 *
 * Documents: https://github.com/hiramtan/HiSocket_unity
 * Author: hiramtan@live.com
***************************************************************/

using System;
using System.Net;
using System.Net.Sockets;

namespace HiSocket
{
    class TcpSocket : SocketBase, ITcp
    {
        public bool IsConnected
        {
            get { return Socket != null && Socket.Connected; }
        }

        private static readonly object _sendLocker = new object();
        private static readonly object _receiveLocker = new object();
        private ByteBlockBuffer _sendBuffer = new ByteBlockBuffer();
        private ByteBlockBuffer _receiveBuffer = new ByteBlockBuffer();
        public TcpSocket(Socket socket) : base(socket)
        {
        }
        public override void Connect(IPEndPoint iep)
        {
            if (IsConnected)
            {
                ErrorEvent("Already Connected");
                return;
            }
            Assert.IsNotNull(iep, "IPEndPoint is null");
            ConnectingEvent();
            try
            {
                Socket.BeginConnect(iep, delegate (IAsyncResult ar)
                {
                    try
                    {
                        var socket = ar.AsyncState as Socket;
                        if (socket == null)
                        {
                            Assert.IsNotNull(socket, "AsyncState as Socket is null");
                        }
                        if (!Socket.Connected)
                        {
                            throw new Exception("Connect faild");
                        }
                        socket.EndConnect(ar);
                        ConnectedEvent();
                        InitThread();
                    }
                    catch (Exception e)
                    {
                        throw new Exception(e.ToString());
                    }

                }, Socket);
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
        }

        public override void Send(byte[] bytes)
        {
            lock (_sendLocker)
            {
                _sendBuffer.WriteAllBytes(bytes);
            }
        }

        protected override void Send()
        {
            while (IsSendThreadOn)
            {
                if (!IsConnected)
                {
                    throw new Exception("From send thread: disconnected");
                }
                lock (_sendLocker)
                {
                    var count = _sendBuffer.Reader.GetHowManyCountCanReadInThisBlock();
                    if (count > 0)
                    {
                        try
                        {
                            var length = Socket.Send(_sendBuffer.Reader.Node.Value, _sendBuffer.Reader.Position, count,
                                SocketFlags.None);
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
            while (IsReceiveThreadOn)
            {
                if (!IsConnected)
                {
                    throw new Exception("From receive thread: disconnected");
                }
                lock (_receiveLocker)
                {
                    if (Socket.Available > 0)
                    {
                        try
                        {
                            var count = _receiveBuffer.Writer.GetHowManyCountCanWriteInThisBlock();
                            var length = Socket.Receive(_receiveBuffer.Writer.Node.Value, _receiveBuffer.Writer.Position,
                                count, SocketFlags.None);
                            _receiveBuffer.Writer.MovePosition(length);
                            var bytes = _receiveBuffer.ReadAllBytes();
                            ReceiveEvent(bytes);
                        }
                        catch (Exception e)
                        {
                            throw new Exception(e.ToString());
                        }
                    }
                }
            }
        }
    }
}
