/***************************************************************
 * Description: Block buffer for reuse array
 * 
 * Documents: https://github.com/hiram3512/HiSocket
 * Support: hiramtan@live.com
***************************************************************/

using System;
using HiSocket;

namespace HiSocket.Tcp.Example
{
    public class ExampleTcpConnection1
    {
        private TcpConnection tcpConnection;
        void Main()
        {
            var package = new ExamplePackage2();
            tcpConnection = new TcpConnection(package);
            tcpConnection.OnConnecting += OnConnecting;
            tcpConnection.OnConnected += OnConnected;
            tcpConnection.OnReceiveMessage += OnReceiveMessage;
            tcpConnection.Connect("127.0.0.1", 999);
        }
        void OnConnecting()
        {
            Console.WriteLine("Connecting");
        }
        void OnConnected()
        {
            Console.WriteLine("Connected");
            var data = new byte[] { 1, 2, 3 };
            tcpConnection.Send(data);
        }
        void OnReceiveMessage(byte[] data)
        {
            Console.WriteLine("Receive data");
        }
    }
}
