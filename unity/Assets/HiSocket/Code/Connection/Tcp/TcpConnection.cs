////****************************************************************************
//// Description:
//// Author: hiramtan@live.com
////***************************************************************************

//using System;
//using System.Net;
//using System.Net.Sockets;

//namespace HiSocket
//{
//    public class TcpConnection : Connection
//    {
//        protected readonly IByteArray _iByteArrayReceive = new ByteArray();
//        protected readonly IByteArray _iByteArraySend = new ByteArray();
//        protected readonly IPackage _iPackage;

//        public TcpConnection(IPackage iPackage)
//        {
//            _iPackage = iPackage;
//        }

//        public override void Connect(string ip, int port)
//        {
//            if (IsConnected)
//            {
//                Console.WriteLine("already connected");
//                return;
//            }
//            ChangeState(SocketState.Connecting);
//            var address = Dns.GetHostAddresses(ip)[0];
//            IPEndPoint iep = new IPEndPoint(address, port);
//            _socket = new Socket(address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
//            _socket.NoDelay = true;
//            _socket.SendTimeout = _socket.ReceiveTimeout = TimeOut;
//            try
//            {
//                _socket.BeginConnect(iep, delegate (IAsyncResult ar)
//                {
//                    var tcp = ar.AsyncState as Socket;
//                    if (tcp != null && tcp.Connected)
//                    {
//                        tcp.EndConnect(ar);
//                        ChangeState(SocketState.Connected);
//                        InitThread();
//                    }
//                    else
//                    {
//                        ChangeState(SocketState.DisConnected);
//                        throw new Exception("tcp connected is false");
//                    }
//                }, _socket);
//            }
//            catch (Exception e)
//            {
//                ChangeState(SocketState.DisConnected);
//                throw new Exception(e.ToString());
//            }
//        }

//        public override void DisConnect()
//        {
//            base.DisConnect();
//            _iByteArraySend.Clear();
//            _iByteArrayReceive.Clear();
//        }

//        protected override void Send()
//        {
//            while (_isSendThreadOn)
//            {
//                if (!IsConnected) //主动or异常断开连接
//                {
//                    ChangeState(SocketState.DisConnected);
//                    throw new Exception("from send: disconnected");
//                }
//                if (_sendQueue.Count > 0)
//                {
//                    try
//                    {
//                        lock (_sendQueue)
//                        {
//                            _iPackage.Pack(_sendQueue, _iByteArraySend);
//                        }
//                    }
//                    catch (Exception e)
//                    {
//                        throw new Exception("pack error: " + e);
//                    }
//                    var toSend = _iByteArraySend.Read(_iByteArraySend.Length);
//                    try
//                    {
//                        _socket.BeginSend(toSend, 0, toSend.Length, SocketFlags.None, delegate (IAsyncResult ar)
//                        {
//                            var tcp = ar.AsyncState as Socket;
//                            var sendLength = tcp.EndSend(ar);
//                            if (sendLength != toSend.Length)
//                            {
//                                //todo: if this will happend, msdn is not handle this issue
//                                throw new Exception("can not send whole bytes at one time");
//                            }
//                        }, _socket);
//                    }
//                    catch (Exception e)
//                    {
//                        throw new Exception(e.ToString());
//                    }

//                }
//            }
//        }
//        protected override void Receive()
//        {
//            while (_isReceiveThreadOn)
//            {
//                if (!IsConnected)
//                {
//                    ChangeState(SocketState.DisConnected);
//                    throw new Exception("from receive: disconnected");
//                }
//                if (_socket.Available > 0)
//                {
//                    try
//                    {
//                        _socket.BeginReceive(ReceiveBuffer, 0, ReceiveBuffer.Length, SocketFlags.None, delegate (IAsyncResult ar)
//                             {
//                                 var tcp = ar.AsyncState as Socket;
//                                 int length = tcp.EndReceive(ar);
//                                 if (length > 0)
//                                 {
//                                     _iByteArrayReceive.Write(ReceiveBuffer, length);
//                                     try
//                                     {
//                                         lock (_receiveQueue)
//                                         {
//                                             _iPackage.Unpack(_iByteArrayReceive, _receiveQueue);
//                                         }
//                                     }
//                                     catch (Exception e)
//                                     {
//                                         throw new Exception(e.ToString());
//                                     }
//                                 }
//                             }, _socket);
//                    }
//                    catch (Exception e)
//                    {
//                        throw new Exception(e.ToString());
//                    }
//                }
//            }
//        }
//    }
//}