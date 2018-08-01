using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace HiSocket.Test
{
    public class TcpServer
    {
        private Socket socket;
        private bool isOn = true;

        public TcpServer()
        {
            Init();
        }

        void Init()
        {
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            IPEndPoint iep = new IPEndPoint(ipAddress, 7777);
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(iep);
            socket.Listen(5);
            socket.NoDelay = true;
            new Thread(Watcher).Start();
        }

        void Watcher()
        {
            byte[] buffer = new byte[1<<30];
            while (isOn)
            {
                var client = socket.Accept();
                isOn = false;
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
            isOn = false;
            socket.Close();
        }
    }
}
