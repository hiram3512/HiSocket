/***************************************************************
 * Description: 
 *
 * Documents: https://github.com/hiramtan/HiSocket
 * Author: hiramtan@live.com
***************************************************************/
using System;
using HiSocket.Tcp;

namespace HiSocket.Example
{
    class Example2
    {
        TcpConnection tcp;
        void Connect()
        {
            tcp = new TcpConnection(new PackageExample());
            tcp.OnDisconnected += OnDisconnect;
            tcp.Connect("127.0.0.1", 999);
            tcp.Socket.NoDelay = true;
            tcp.Socket.SendTimeout = 100;
            tcp.Socket.ReceiveTimeout = 200;
            //...


            // you can add plugin sub from IPlugins
            tcp.AddPlugin(new StatisticalPlugin("Statistical"));//this plugin calculate how many send
        }

        void OnDisconnect()
        {
            var length = tcp.SendBuffer.WritePosition;
            Console.WriteLine("Still have {0} not send to server when abnormal shutdown");
            var data = tcp.SendBuffer.Read(length);
            tcp.SendBuffer.ResetIndex();

            //use can handle these data, for example maybe can send next time when connect again
            //tcp.Send(data);
        }
    }
}
