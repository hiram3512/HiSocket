//****************************************************************************
// Description:
// Author: hiramtan@live.com
//****************************************************************************

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;
using Ping = System.Net.NetworkInformation.Ping;

namespace HiSocket
{
    public abstract class Connection
    {
        protected Socket _socket;
        protected int _receiveBufferSize = 1024 * 128; //128k
        protected byte[] ReceiveBuffer;
        protected Queue<byte[]> _receiveQueue = new Queue<byte[]>();
        protected Queue<byte[]> _sendQueue = new Queue<byte[]>();
        public int ReceiveBufferSize
        {
            get { return _receiveBufferSize; }
            set
            {
                _receiveBufferSize = value;
                ReceiveBuffer = new byte[ReceiveBufferSize];
            }
        }
        public event Action<byte[]> ReceiveEvent;
        protected Connection()
        {
            Debug.Log("you can download newest version from here: https://github.com/hiramtan/HiSocket_unity");
            ReceiveBuffer = new byte[_receiveBufferSize];
        }
        public void Run()
        {
            while (_receiveQueue.Count > 0)
            {
                if (ReceiveEvent != null)
                {
                    ReceiveEvent(_receiveQueue.Dequeue());
                }
            }
        }
        public abstract void Connect(string ip, int port);
        protected abstract void Send();
        protected abstract void Receive();
        public void Send(byte[] bytes)
        {
            lock (_sendQueue)
            {
                _sendQueue.Enqueue(bytes);
            }
        }

        /// <summary>
        ///     bug there will be a bug if you .net is 2.0sub
        /// </summary>
        /// <returns></returns>
        public long Ping(string ip)
        {
            var ipAddress = IPAddress.Parse(ip);
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
        public virtual void DisConnect()
        {
            AbortThread();
            lock (_sendQueue)
            {
                _sendQueue.Clear();
            }
            lock (_receiveQueue)
            {
                _receiveQueue.Clear();
            }
            _socket.Shutdown(SocketShutdown.Both);
            _socket.Close();
            _socket = null;
        }

        protected bool _isSendThreadOn;
        protected bool _isReceiveThreadOn;
        private Thread sendThread;
        private Thread receiveThread;
        protected void InitThread()
        {
            _isSendThreadOn = true;
            sendThread = new Thread(Send);
            sendThread.Start();
            _isReceiveThreadOn = true;
            receiveThread = new Thread(Receive);
            receiveThread.Start();
        }
        private void AbortThread()
        {
            try
            {
                _isSendThreadOn = false;
                if (sendThread != null)
                    sendThread.Abort();
                sendThread = null;
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
            try
            {
                _isReceiveThreadOn = false;
                if (receiveThread != null)
                    receiveThread.Abort();
                receiveThread = null;
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
        }
    }
}