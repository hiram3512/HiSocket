//****************************************************************************
// Description:
// Author: hiramtan@qq.com
//***************************************************************************

//#define NoThread
#define SingleThread
//#define MultiThread



#if MultiThread //havent finish
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace HiSocket.Tcp
{
    public class TcpClient : ISocket
    {
        public int TimeOut { get; set; }
        public int ReceiveBufferSize { get; set; }
        public Action<SocketState> StateEvent { get; set; }
        public bool IsConnected { get; }

        private System.Net.Sockets.TcpClient tcp;
        private IProto iPackage;
        public void Connect(string ip, int port)
        {
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


        private void InitThread()
        {
            Thread sendThread = new Thread(Send);
            sendThread.Start();


        }


        Queue<byte[]> sendQueue = new Queue<byte[]>();
        MemoryStream sendMS = new MemoryStream();
        private bool isSendThreadOn;
        private void Send()
        {
            while (isSendThreadOn)
            {
                if (!IsConnected)//主动or异常断开连接
                    break;

                lock (sendQueue)
                {
                    if (sendQueue.Count > 0)
                    {
                        var msg = sendQueue.Dequeue();
                        sendMS.Seek(0, SeekOrigin.End);
                        sendMS.Write(msg, 0, msg.Length);
                        sendMS.Seek(0, SeekOrigin.Begin);
                        iPackage.Pack(sendMS);
                        var toSend = sendMS.GetBuffer();
                        tcp.Client.BeginSend(toSend, 0, toSend.Length, SocketFlags.None, delegate (IAsyncResult ar)
                         {
                             try
                             {
                                 System.Net.Sockets.TcpClient tcp = ar.AsyncState as System.Net.Sockets.TcpClient;
                                 tcp.EndConnect(ar);
                             }
                             catch (Exception e)
                             {
                                 Console.WriteLine(e);
                                 throw;
                             }
                         }, tcp);
                    }
                }
            }
        }
    }
}
#endif


#if SingleThread //havent finish
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using HiSocket.Msg;

namespace HiSocket.Tcp
{
    // State object for receiving data from remote device.  
    public class TcpClient : ISocket
    {
        public int TimeOut
        {
            private get { return _timeOut; }
            set { _timeOut = value; }
        }

        public int ReceiveBufferSize
        {
            private get { return _receiveBufferSize; }
            set
            {
                _receiveBufferSize = value;
                _receiveBuffer = new byte[ReceiveBufferSize];
            }
        }

        public Action<SocketState> StateEvent { get; set; }
        public bool IsConnected { get { return _client != null && _client.Client != null && _client.Connected; } }

        private string _ip;
        private int _port;
        private IPackage _iPackage;
        private System.Net.Sockets.TcpClient _client;
        private int _receiveBufferSize = 1024 * 128;//128k
        private byte[] _receiveBuffer;
        private int _timeOut = 5000;//5s:收发超时时间
        private readonly IByteArray _iByteArraySend = new ByteArray();
        private readonly IByteArray _iByteArrayReceive = new ByteArray();



        private static ManualResetEvent connectDone =
            new ManualResetEvent(false);
        private static ManualResetEvent sendDone =
            new ManualResetEvent(false);
        private static ManualResetEvent receiveDone =
            new ManualResetEvent(false);


        public TcpClient(IPackage iPackage)
        {
            _receiveBuffer = new byte[ReceiveBufferSize];
            this._iPackage = iPackage;
            _client = new System.Net.Sockets.TcpClient();
            _client.NoDelay = true;
            _client.SendTimeout = _client.ReceiveTimeout = TimeOut;
        }
        private void ChangeState(SocketState state)
        {
            if (StateEvent != null)
            {
                StateEvent(state);
            }
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
                _client.BeginConnect(ip, port,
                    new AsyncCallback((x) =>
                    {
                        try
                        {
                            System.Net.Sockets.TcpClient client = x.AsyncState as System.Net.Sockets.TcpClient;
                            client.EndConnect(x);
                            connectDone.Set();
                            ChangeState(SocketState.Connected);
                        }
                        catch (Exception e)
                        {
                            throw new Exception(e.ToString());
                        }
                    }), _client);
                connectDone.WaitOne();
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }


        }

        public void DisConnect()
        {
            throw new NotImplementedException();
        }

        public void Send(byte[] bytes)
        {
            _client.Client.BeginSend(bytes, 0, bytes.Length, SocketFlags.None,
                new AsyncCallback((x) =>
                {

                }), _client);
            sendDone.WaitOne();
        }

        public long Ping()
        {
            IPAddress ipAddress = IPAddress.Parse(_ip);
            System.Net.NetworkInformation.Ping tempPing = new System.Net.NetworkInformation.Ping();
            System.Net.NetworkInformation.PingReply temPingReply = tempPing.Send(ipAddress);
            return temPingReply.RoundtripTime;
        }
    }

    public class AsynchronousClient
    {
        // The port number for the remote device.  
        //private const int port = 11000;

        // ManualResetEvent instances signal completion.  
        //private static ManualResetEvent connectDone =
        //    new ManualResetEvent(false);
        //private static ManualResetEvent sendDone =
        //    new ManualResetEvent(false);
        //private static ManualResetEvent receiveDone =
        //    new ManualResetEvent(false);

        // The response from the remote device.  
        private static String response = String.Empty;

        private static void StartClient()
        {
            // Connect to a remote device.  
            try
            {
                // Establish the remote endpoint for the socket.  
                // The name of the   
                // remote device is "host.contoso.com".  
                //IPHostEntry ipHostInfo = Dns.Resolve("host.contoso.com");
                //IPAddress ipAddress = ipHostInfo.AddressList[0];
                //IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);

                //// Create a TCP/IP socket.  
                //Socket client = new Socket(AddressFamily.InterNetwork,
                //    SocketType.Stream, ProtocolType.Tcp);

                // Connect to the remote endpoint.  
                //client.BeginConnect(remoteEP,
                //    new AsyncCallback(ConnectCallback), client);
                //connectDone.WaitOne();

                // Send test data to the remote device.  
                //Send(client, "This is a test<EOF>");
                //sendDone.WaitOne();

                // Receive the response from the remote device.  
                Receive(client);
                receiveDone.WaitOne();

                // Write the response to the console.  
                Console.WriteLine("Response received : {0}", response);

                // Release the socket.  
                client.Shutdown(SocketShutdown.Both);
                client.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private static void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  
                Socket client = (Socket)ar.AsyncState;

                // Complete the connection.  
                client.EndConnect(ar);

                Console.WriteLine("Socket connected to {0}",
                    client.RemoteEndPoint.ToString());

                // Signal that the connection has been made.  
                connectDone.Set();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private static void Receive(Socket client)
        {
            try
            {
                // Create the state object.  
                TcpClient state = new TcpClient();
                state.workSocket = client;

                // Begin receiving the data from the remote device.  
                client.BeginReceive(state.buffer, 0, TcpClient.BufferSize, 0,
                    new AsyncCallback(ReceiveCallback), state);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private static void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the state object and the client socket   
                // from the asynchronous state object.  
                TcpClient state = (TcpClient)ar.AsyncState;
                Socket client = state.workSocket;

                // Read data from the remote device.  
                int bytesRead = client.EndReceive(ar);

                if (bytesRead > 0)
                {
                    // There might be more data, so store the data received so far.  
                    state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));

                    // Get the rest of the data.  
                    client.BeginReceive(state.buffer, 0, TcpClient.BufferSize, 0,
                        new AsyncCallback(ReceiveCallback), state);
                }
                else
                {
                    // All the data has arrived; put it in response.  
                    if (state.sb.Length > 1)
                    {
                        response = state.sb.ToString();
                    }
                    // Signal that all bytes have been received.  
                    receiveDone.Set();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private static void Send(Socket client, String data)
        {
            // Convert the string data to byte data using ASCII encoding.  
            byte[] byteData = Encoding.ASCII.GetBytes(data);

            // Begin sending the data to the remote device.  
            client.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), client);
        }

        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  
                Socket client = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.  
                int bytesSent = client.EndSend(ar);
                Console.WriteLine("Sent {0} bytes to server.", bytesSent);

                // Signal that all bytes have been sent.  
                sendDone.Set();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public static int Main(String[] args)
        {
            StartClient();
            return 0;
        }
    }
}
#endif


#if NoThread
using HiSocket.Msg;
using System;
using System.Net;
using System.Net.Sockets;

namespace HiSocket.Tcp
{
    public class TcpClient : ISocket
    {
        public int TimeOut
        {
            private get { return _timeOut; }
            set { _timeOut = value; }
        }

        public int ReceiveBufferSize
        {
            private get { return _receiveBufferSize; }
            set
            {
                _receiveBufferSize = value;
                _receiveBuffer = new byte[ReceiveBufferSize];
            }
        }

        public Action<SocketState> StateEvent { get; set; }
        public bool IsConnected { get { return _client != null && _client.Client != null && _client.Connected; } }

        private string _ip;
        private int _port;
        private IPackage _iPackage;
        private System.Net.Sockets.TcpClient _client;
        private int _receiveBufferSize = 1024 * 128;//128k
        private byte[] _receiveBuffer;
        private int _timeOut = 5000;//5s:收发超时时间
        private readonly IByteArray _iByteArraySend = new ByteArray();
        private readonly IByteArray _iByteArrayReceive = new ByteArray();
        public TcpClient(IPackage iPackage)
        {
            _receiveBuffer = new byte[ReceiveBufferSize];
            this._iPackage = iPackage;
            _client = new System.Net.Sockets.TcpClient();
            _client.NoDelay = true;
            _client.SendTimeout = _client.ReceiveTimeout = TimeOut;
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
                this._client.BeginConnect(ip, port, (delegate (IAsyncResult ar)
                {
                    try
                    {
                        System.Net.Sockets.TcpClient tcp = ar.AsyncState as System.Net.Sockets.TcpClient;
                        tcp.EndConnect(ar);
                        if (tcp.Connected)
                        {
                            ChangeState(SocketState.Connected);
                            tcp.Client.BeginReceive(_receiveBuffer, 0, _receiveBuffer.Length, SocketFlags.None, Receive, tcp);
                        }
                        else ChangeState(SocketState.DisConnected);
                    }
                    catch (Exception e)
                    {
                        ChangeState(SocketState.DisConnected);
                        throw new Exception(e.ToString());
                    }
                }), _client);
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
                _iByteArraySend.Clear();
                _iByteArraySend.Write(bytes, bytes.Length);
                _iPackage.Pack(_iByteArraySend);
                var toSend = _iByteArraySend.Read(_iByteArraySend.Length);
                _client.Client.BeginSend(toSend, 0, toSend.Length, SocketFlags.None, delegate (IAsyncResult ar)
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
                }, _client);
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
                    _iByteArrayReceive.Write(_receiveBuffer, length);
                    _iPackage.Unpack(_iByteArrayReceive);
                }
                tcp.Client.BeginReceive(_receiveBuffer, 0, _receiveBuffer.Length, SocketFlags.None, Receive, tcp);
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
                _client.Client.Shutdown(SocketShutdown.Both);
                _client.Close();
                _client = null;
            }
            ChangeState(SocketState.DisConnected);
            StateEvent = null;
        }
        public long Ping()
        {
            IPAddress ipAddress = IPAddress.Parse(_ip);
            System.Net.NetworkInformation.Ping tempPing = new System.Net.NetworkInformation.Ping();
            System.Net.NetworkInformation.PingReply temPingReply = tempPing.Send(ipAddress);
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
#endif