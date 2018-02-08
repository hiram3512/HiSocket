using UnityEngine;
using System.Collections;

namespace HiSocket
{
    public class Tcp : Connection, ITcp
    {
        public override int ReceiveBufferSize { get; set; }

        public override void Connect(string ip, int port)
        {
            throw new System.NotImplementedException();
        }

        protected override void Send()
        {
            throw new System.NotImplementedException();
        }

        protected override void Receive()
        {
            throw new System.NotImplementedException();
        }
    }
}