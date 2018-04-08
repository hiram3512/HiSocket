using System;
using System.Net;
using System.Net.Sockets;

namespace TestServer
{
    class Program
    {
        private Socket _socket;
        byte[] buffer = new byte[1024];
        static void Main(string[] args)
        {
            new Program().Start();
        }

        public void Start()
        {
            IPAddress _ipAddress = IPAddress.Parse("127.0.0.1");
            IPEndPoint iep = new IPEndPoint(_ipAddress, 7777);
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _socket.Bind(iep);
            _socket.Listen(2);
            _socket.NoDelay = true;
            while (true)
            {
                var client = _socket.Accept();
                while (true)
                {
                    var len = client.Receive(buffer);
                    byte[] toSend = new byte[len];
                    Array.Copy(buffer, 0, toSend, 0, toSend.Length);
                    Console.WriteLine(toSend.Length);
                    client.Send(toSend);
                }
            }
        }
    }
}