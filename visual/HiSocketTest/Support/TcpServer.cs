using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace HiSocketTest
{
    public class TcpServer
    {
        private Socket _socket;
        private bool _isOn = true;

        public TcpServer()
        {
            Init();
        }

        void Init()
        {
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            IPEndPoint iep = new IPEndPoint(ipAddress, 7777);
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _socket.Bind(iep);
            _socket.Listen(5);
            _socket.NoDelay = true;
            new Thread(Watcher).Start();
        }

        void Watcher()
        {
            byte[] buffer = new byte[1024];
            while (_isOn)
            {
                var client = _socket.Accept();
                int length = 0;
                while ((length = client.Receive(buffer)) > 0)
                {
                    byte[] toSend = new byte[length];
                    Array.Copy(buffer, 0, toSend, 0, toSend.Length);
                    Console.WriteLine(toSend.Length);
                    client.Send(toSend);
                }
            }
        }

        public void Close()
        {
            _isOn = false;
            _socket.Close();
        }
    }
}
