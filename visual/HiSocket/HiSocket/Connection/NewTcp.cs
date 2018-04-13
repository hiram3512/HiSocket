using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace HiSocket
{
    class NewTcp : NewConnection, ITcp
    {
        public bool IsConnected
        {
            get { return Socket != null && Socket.Connected; }
        }

        private static readonly object _sendLocker = new object();
        private static readonly object _receiveLocker = new object();
        private ByteBlockBuffer _sendBuffer = new ByteBlockBuffer();
        private ByteBlockBuffer _receiveBuffer = new ByteBlockBuffer();
        public NewTcp(Socket socket) : base(socket)
        {
        }
        public override void Connect(IPEndPoint iep)
        {
            MakeSureNotNull(iep, "IPEndPoint is null");
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
                            MakeSureNotNull(socket, "AsyncState as Socket is null");
                        }
                        if (!Socket.Connected)
                        {
                            ErrorEvent(new Exception("Connect faild"));
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
        protected override void Send()
        {
            while (IsSendThreadOn)
            {
                if (!IsConnected)
                {
                    throw new Exception("From send thread: disconnected");
                }
                var count = _sendBuffer.Reader.GetHowManyCountCanReadInThisBlock();
                Socket.Send()
            }
        }

        protected override void Receive()
        {

        }


    }
}
