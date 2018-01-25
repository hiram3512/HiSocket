//****************************************************************************
// Description:
// Author: hiramtan@live.com
//****************************************************************************

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;

namespace HiSocket
{
    public abstract class Connection : ISocket
    {
        protected int _receiveBufferSize = 1024 * 128; //128k
        protected int _timeOut = 5000;
        protected string IP;
        protected int Port;
        protected byte[] ReceiveBuffer;

        protected Queue<byte[]> _receiveQueue = new Queue<byte[]>();
        protected Queue<byte[]> _sendQueue = new Queue<byte[]>();

        protected Connection()
        {
            ReceiveBuffer = new byte[_receiveBufferSize];
        }

        public abstract int TimeOut { get; set; }

        public int ReceiveBufferSize
        {
            get
            {
                return _receiveBufferSize;
            }
            set
            {
                _receiveBufferSize = value;
                ReceiveBuffer = new byte[ReceiveBufferSize];
            }
        }

        public Action<SocketState> StateChangeHandler { protected get; set; }
        public Action<byte[]> ReceiveHandler { get; set; }

        public void Run()
        {
            while (_receiveQueue.Count > 0)
            {
                if (ReceiveHandler != null)
                {
                    ReceiveHandler(_receiveQueue.Dequeue());
                }
            }
        }

        public abstract bool IsConnected { get; }

        public abstract void Connect(string ip, int port);

        public abstract void Send(byte[] bytes);

        /// <summary>
        ///     bug there will be a bug if you .net is 2.0sub
        /// </summary>
        /// <returns></returns>
        public long Ping()
        {
            //如果unity选择.net为2.0sub会出现bug
            //如果unity选择.net为4.6不会出现
            var ipAddress = IPAddress.Parse(IP);
            var tempPing = new Ping();
            var temPingReply = tempPing.Send(ipAddress);
            return temPingReply.RoundtripTime;


            //private int pingTime;
            //private Ping p;
            //private float timeOut = 1;
            //private float lastTime;
            //void Start()
            //{
            //    StartCoroutine(Ping());
            //}
            //IEnumerator Ping()
            //{
            //    p = new Ping("127.0.0.1");
            //    lastTime = Time.realtimeSinceStartup;
            //    while (!p.isDone && Time.realtimeSinceStartup - lastTime < 1)
            //    {
            //        yield return null;
            //    }
            //    pingTime = p.time;
            //    p.DestroyPing();
            //    yield return new WaitForSeconds(1);
            //    StartCoroutine(Ping());
            //    }
        }
        //public long Ping()
        //{
        //    IPAddress ipAddress = IPAddress.Parse(IP);
        //    System.Net.NetworkInformation.Ping tempPing = new System.Net.NetworkInformation.Ping();
        //    System.Net.NetworkInformation.PingReply temPingReply = tempPing.Send(ipAddress);
        //    return temPingReply.RoundtripTime;
        //}
        public abstract void DisConnect();

        protected void ChangeState(SocketState state)
        {
            if (StateChangeHandler != null)
                StateChangeHandler(state);
        }
    }
}