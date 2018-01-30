using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TestServer
{
    class Program
    {
        private Socket _socket;
        byte[] bytes = new byte[1024];
        static void Main(string[] args)
        {
            new Program().Start();
        }

        public void Start()
        {
            IPAddress _ipAddress = IPAddress.Parse("127.0.0.1");
            IPEndPoint iep = new IPEndPoint(_ipAddress, 5077);
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _socket.Bind(iep);
            _socket.Listen(2);
            while (true)
            {
                var client = _socket.Accept();
                if (client != null)
                {
                    while (client.Receive(bytes) > 0)
                    {
                        client.Send(bytes);
                    }
                }
            }
        }
    }
}