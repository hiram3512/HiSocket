//****************************************************************************
// Description:
// Author: hiramtan@live.com
//****************************************************************************
using System;
using System.Net;
using System.Net.NetworkInformation;

namespace HiSocket
{
    public abstract class Connection : ISocket
    {
        protected static int _receiveBufferSize = 1024 * 128; //128k
        protected readonly IByteArray _iByteArrayReceive = new ByteArray();
        protected readonly IByteArray _iByteArraySend = new ByteArray();
        protected readonly IPackage _iPackage;
        protected string _ip;
        protected int _port;
        protected byte[] _receiveBuffer = new byte[_receiveBufferSize];
        public Connection(IPackage iPackage)
        {
            _iPackage = iPackage;
        }

        private int _timeOut = 5000;
        public int TimeOut
        {
            get { return _timeOut; }
            set { _timeOut = value; }
        }

        public int ReceiveBufferSize
        {
            get { return _receiveBufferSize; }
            set
            {
                _receiveBufferSize = value;
                _receiveBuffer = new byte[ReceiveBufferSize];
            }
        }

        public Action<SocketState> StateChangeHandler { protected get; set; }
        public abstract bool IsConnected { get; }

        public abstract void Connect(string ip, int port);

        public abstract void Send(byte[] bytes);

        public long Ping()
        {
            //如果unity选择.net为2.0sub会出现bug
            //如果unity选择.net为4.6不会出现
            var ipAddress = IPAddress.Parse(_ip);
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
        protected void ChangeState(SocketState state)
        {
            if(StateChangeHandler!=null)
            StateChangeHandler(state);
        }
        public abstract void DisConnect();
    }
}
