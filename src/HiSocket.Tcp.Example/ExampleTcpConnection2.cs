/***************************************************************
 * Description: Block buffer for reuse array
 * 
 * Documents: https://github.com/hiram3512/HiSocket
 * Support: hiramtan@live.com
***************************************************************/

using System;
using System.Collections.Generic;
using System.Text;

namespace HiSocket.Tcp.Example
{
    public class ExampleTcpConnection2
    {
        private TcpConnection tcpConnection;
        void Main()
        {
            var package = new ExamplePackage2();
            tcpConnection = new TcpConnection(package);
            tcpConnection.OnConnected += OnConnected;
            tcpConnection.OnReceiveMessage += OnReceiveMessage;
            tcpConnection.OnException += OnException;
            tcpConnection.Connect("127.0.0.1", 999);
            tcpConnection.Socket.NoDelay = true;
            tcpConnection.Socket.SendTimeout = 100;
            tcpConnection.Socket.ReceiveTimeout = 200;
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
        void OnException(Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }
}
