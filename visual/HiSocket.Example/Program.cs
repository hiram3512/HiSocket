using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiSocket.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            PackageExample package = new PackageExample();
            TcpConnection tcp = new TcpConnection(package);
            tcp.OnReceive += OnReceive;
            tcp.OnConnecting += OnConnecting;
            //
            //
        }

        static void OnReceive(byte[] bytes)
        {
            Console.WriteLine("Get message from server and length is " + bytes.Length);
        }

        static void OnDisconnect()
        {

        }

        static void OnConnecting()
        {

        }
    }
}
