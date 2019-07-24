/***************************************************************
 * Description: 
 *
 * Documents: https://github.com/hiramtan/HiSocket
 * Author: hiramtan@live.com
***************************************************************/

using System;

namespace HiSocket.Example
{
    class TcpSocketExample
    {
        private static TcpConnection tcp;
        static void Main(string[] args)
        {

            PackageExample package = new PackageExample();
            tcp = new TcpConnection(package);
            tcp.OnReceiveMessage += OnReceive;
            tcp.OnConnecting += OnConnecting;
            //
            //
        }

        void Button_Connect()
        {
            tcp.Connect("127.0.0.1", 999);
        }

        void Button_Disconnect()
        {
            tcp.Close();
        }

        void Button_Send()
        {
            byte[] data = System.Text.Encoding.UTF8.GetBytes("hello");
            tcp.Send(data);
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
