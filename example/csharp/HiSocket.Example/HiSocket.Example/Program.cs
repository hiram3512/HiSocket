using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HiSocket.Tcp;
using HiSocket.Test;

namespace HiSocket.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            ITcpConnection tcp = new TcpConnection(new Package());
            tcp.OnSendMessage += (x) => { };
            tcp.OnReceiveMessage += (x) => {  };
            tcp.Connect("127.0.0.1",7777);
            tcp.Send(new byte[10]);
        }
    }
}
