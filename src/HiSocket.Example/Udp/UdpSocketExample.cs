/***************************************************************
 * Description: 
 *
 * Documents: https://github.com/hiramtan/HiSocket
 * Author: hiramtan@live.com
***************************************************************/

using System;

namespace HiSocket.Example
{
    class UdpSocketExample
    {
        private UdpConnection udp;
        void Main()
        {
            udp = new UdpConnection();
            udp.OnReceiveMessage += (x => { Console.WriteLine("receive data length" + x.Length); });

        }

        void Button_Connect()
        {
            udp.Connect("127.0.0.1", 999);
        }
    }
}
