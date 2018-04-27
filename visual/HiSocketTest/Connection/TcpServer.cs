using System;
using System.Net;
using System.Net.Sockets;

namespace HiSocketTest
{
    public class TcpServer
    {

        void Init()
        {
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            IPEndPoint iep = new IPEndPoint(ipAddress, 7777);
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(iep);
            socket.Listen(5);
            socket.NoDelay = true;
            byte[] buffer = new byte[2048];
            while (true)
            {
                var client = socket.Accept();
                int length = 0;
                while ((length = client.Receive(buffer)) > 0)
                {
                    ;
                    byte[] toSend = new byte[length];
                    Array.Copy(buffer, 0, toSend, 0, toSend.Length);
                    Console.WriteLine(toSend.Length);
                    client.Send(toSend);
                }
            }
        }
    }
}
