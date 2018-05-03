/***************************************************************
 * Description:
 *
 * Documents: https://github.com/hiramtan/HiSocket_unity
 * Author: hiramtan@live.com
***************************************************************/

using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace HiSocket
{
    public class TcpSocket : SocketBase, ITcp
    {
        public bool IsConnected
        {
            get { return Socket != null && Socket.Connected; }
        }
        /// <summary>
        /// For send data
        /// </summary>
        private Thread sendThread;
        /// <summary>
        /// For receive data
        /// </summary>
        private Thread receiveThread;

        /// <summary>
        /// If send thread is run
        /// </summary>
        private bool IsSendThreadOn;
        /// <summary>
        /// If receive thread is run
        /// </summary>
        private bool IsReceiveThreadOn;

        private static readonly object _sendLocker = new object();
        private static readonly object _receiveLocker = new object();
        private IByteBlockBuffer _sendBuffer = new ByteBlockBuffer();
        private IByteBlockBuffer _receiveBuffer = new ByteBlockBuffer();

        public override void Connect(IPEndPoint iep)
        {
            if (IsConnected)
            {
                ErrorEvent("Already Connected");
                return;
            }
            Assert.NotNull(iep, "IPEndPoint is null");
            ConnectingEvent();
            Socket = new Socket(iep.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                Socket.BeginConnect(iep, delegate (IAsyncResult ar)
                {
                    try
                    {
                        var socket = ar.AsyncState as Socket;
                        Assert.NotNull(socket, "Socket is null when connect end");
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

        public override void DisConnect()
        {
            base.DisConnect();
            AbortThread();
        }

        private void Send()
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

        private void Receive()
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
        private void InitThread()
        {
            IsSendThreadOn = true;
            sendThread = new Thread(Send);
            sendThread.Start();
            IsReceiveThreadOn = true;
            receiveThread = new Thread(Receive);
            receiveThread.Start();
        }

        private void AbortThread()
        {
            try
            {
                IsReceiveThreadOn = false;
                if (sendThread != null)
                    sendThread.Abort();
                sendThread = null;
            }
            catch (Exception e)
            {
                throw new Exception("Abort send thread with error: " + e.ToString());
            }
            try
            {
                IsReceiveThreadOn = false;
                if (receiveThread != null)
                    receiveThread.Abort();
                receiveThread = null;
            }
            catch (Exception e)
            {
                throw new Exception("Abort receive thread with error: " + e.ToString());
            }
        }
    }
}
