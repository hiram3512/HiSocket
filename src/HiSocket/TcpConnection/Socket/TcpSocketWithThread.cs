///***************************************************************
// * Description: because recevie data is in thread, user should operate unity's component main thread.
// * If you want socket handle in thread, you can use this logic
// *
// * Documents: https://github.com/hiramtan/HiSocket
// * Author: hiramtan@live.com
//***************************************************************/

//using System;
//using System.Net;
//using System.Net.Sockets;
//using System.Threading;
//using HiFramework;

//namespace HiSocket
//{
//    public class TcpSocketWithThread : ITcpSocket
//    {
//        public Socket Socket { get; protected set; }
//        public event Action OnConnected;
//        public event Action OnConnecting;
//        public event Action OnDisconnected;
//        public event Action<byte[]> OnSocketReceive;
//        public event Action<byte[]> OnSocketSend;
//        public event Action<Exception> OnError;
//        public bool IsConnected
//        {
//            get { return Socket != null && Socket.Connected; }
//        }
//        /// <summary>
//        /// For send data
//        /// </summary>
//        private Thread sendThread;
//        /// <summary>
//        /// For receive data
//        /// </summary>
//        private Thread receiveThread;

//        /// <summary>
//        /// If send thread is run
//        /// </summary>
//        private bool IsSendThreadOn;
//        /// <summary>
//        /// If receive thread is run
//        /// </summary>
//        private bool IsReceiveThreadOn;

//        private static readonly object sendLocker = new object();
//        private static readonly object receiveLocker = new object();
//        private IByteBlockBuffer sendBuffer = new ByteBlockBuffer();
//        private IByteBlockBuffer receiveBuffer = new ByteBlockBuffer();

//        public void Connect(IPEndPoint iep)
//        {
//            if (IsConnected)
//            {
//                ErrorEvent("Already Connected");
//                return;
//            }
//            AssertThat.IsNotNull(iep);
//            ConnectingEvent();
//            Socket = new Socket(iep.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
//            try
//            {
//                Socket.BeginConnect(iep, delegate (IAsyncResult ar)
//                {
//                    try
//                    {
//                        var socket = ar.AsyncState as Socket;
//                        AssertThat.IsNotNull(socket);
//                        if (!Socket.Connected)
//                        {
//                            throw new Exception("Connect faild");
//                        }
//                        socket.EndConnect(ar);
//                        ConnectedEvent();
//                        InitThread();
//                    }
//                    catch (Exception e)
//                    {
//                        throw new Exception(e.ToString());
//                    }

//                }, Socket);
//            }
//            catch (Exception e)
//            {
//                throw new Exception(e.ToString());
//            }
//        }

//        /// <summary>
//        /// Connect to server
//        /// </summary>
//        /// <param name="ip">ipv4/ipv6</param>
//        /// <param name="port"></param>
//        public void Connect(string ip, int port)
//        {
//            var iep = new IPEndPoint(IPAddress.Parse(ip), port);
//            Connect(iep);
//        }

//        /// <summary>
//        /// Connect to server
//        /// </summary>
//        /// <param name="ip"></param>
//        /// <param name="port"></param>
//        public void Connect(IPAddress ip, int port)
//        {
//            var iep = new IPEndPoint(ip, port);
//            Connect(iep);
//        }

//        public void Send(byte[] bytes)
//        {
//            lock (sendLocker)
//            {
//                sendBuffer.WriteAllBytes(bytes);
//            }
//        }

//        /// <summary>
//        /// Send bytes to server
//        /// </summary>
//        /// <param name="bytes"></param>
//        public void Send(ArraySegment<byte> bytes)
//        {
//          Send(bytes.Array);
//        }

//        public void DisConnect()
//        {
//            try
//            {
//                AbortThread();
//                Socket.Shutdown(SocketShutdown.Both);
//                Socket.Close();
//                Socket = null;
//            }
//            catch (Exception e)
//            {
//                throw new Exception("Disconnect with error: " + e.ToString());
//            }
//            DisconnectedEvnet();
//        }

//        private void Send()
//        {
//            while (IsSendThreadOn)
//            {
//                if (!IsConnected)
//                {
//                    throw new Exception("From send thread: disconnected");
//                }
//                lock (sendLocker)
//                {
//                    var count = sendBuffer.Reader.GetHowManyCountCanReadInThisBlock();
//                    if (count > 0)
//                    {
//                        try
//                        {
//                            var length = Socket.Send(sendBuffer.Reader.Node.Value, sendBuffer.Reader.Position, count,
//                                SocketFlags.None);
//                            byte[] sendBytes = new byte[length];
//                            Array.Copy(sendBuffer.Reader.Node.Value, sendBuffer.Reader.Position, sendBytes, 0, sendBytes.Length);
//                            SocketSendEvent(sendBytes);
//                            sendBuffer.Reader.MovePosition(length);
//                        }
//                        catch (Exception e)
//                        {
//                            throw new Exception(e.ToString());
//                        }
//                    }
//                }
//            }
//        }

//        private void Receive()
//        {
//            while (IsReceiveThreadOn)
//            {
//                if (!IsConnected)
//                {
//                    throw new Exception("From receive thread: disconnected");
//                }
//                lock (receiveLocker)
//                {
//                    if (Socket.Available > 0)
//                    {
//                        try
//                        {
//                            var count = receiveBuffer.Writer.GetHowManyCountCanWriteInThisBlock();
//                            var length = Socket.Receive(receiveBuffer.Writer.Node.Value, receiveBuffer.Writer.Position,
//                                count, SocketFlags.None);
//                            receiveBuffer.Writer.MovePosition(length);
//                            var bytes = receiveBuffer.ReadAllBytes();
//                            SocketReceiveEvent(bytes);
//                        }
//                        catch (Exception e)
//                        {
//                            throw new Exception(e.ToString());
//                        }
//                    }
//                }
//            }
//        }
//        private void InitThread()
//        {
//            IsSendThreadOn = true;
//            sendThread = new Thread(Send);
//            sendThread.Start();
//            IsReceiveThreadOn = true;
//            receiveThread = new Thread(Receive);
//            receiveThread.Start();
//        }

//        private void AbortThread()
//        {
//            try
//            {
//                IsReceiveThreadOn = false;
//                if (sendThread != null)
//                    sendThread.Abort();
//                sendThread = null;
//            }
//            catch (Exception e)
//            {
//                throw new Exception("Abort send thread with error: " + e.ToString());
//            }
//            try
//            {
//                IsReceiveThreadOn = false;
//                if (receiveThread != null)
//                    receiveThread.Abort();
//                receiveThread = null;
//            }
//            catch (Exception e)
//            {
//                throw new Exception("Abort receive thread with error: " + e.ToString());
//            }
//        }
//        protected void ConnectingEvent()
//        {
//            if (OnConnecting != null)
//            {
//                OnConnecting();
//            }
//        }
//        protected void ErrorEvent(string info)
//        {
//            if (OnError != null)
//            {
//                OnError(new Exception(info));
//            }
//        }

//        protected void ConnectedEvent()
//        {
//            if (OnConnected != null)
//            {
//                OnConnected();
//            }
//        }

//        protected void SocketReceiveEvent(byte[] bytes)
//        {
//            if (OnSocketReceive != null)
//            {
//                OnSocketReceive(bytes);
//            }
//        }

//        private void DisconnectedEvnet()
//        {
//            if (OnDisconnected != null)
//            {
//                OnDisconnected();
//            }
//        }

//        private void SocketSendEvent(byte[] bytes)
//        {
//            if (OnSocketSend != null)
//            {
//                OnSocketSend(bytes);
//            }
//        }

//        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
//        public void Dispose()
//        {
//            DisConnect();

//            Socket = null;
//            OnConnecting = null;
//            OnConnected = null;
//            OnDisconnected = null;
//            OnSocketReceive = null;
//            OnSocketSend = null;
//            sendBuffer = null;
//            receiveBuffer = null;
//        }
//    }
//}
