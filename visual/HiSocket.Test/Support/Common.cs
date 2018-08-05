using System.Net;
using System.Threading;

namespace HiSocket.Test
{
    class Common
    {
        public static IPEndPoint GetIpEndPoint()
        {
            return new IPEndPoint(IPAddress.Parse("127.0.0.1"), 7777);
        }

        public static void WaitConnected(ITcpSocket tcp, int timeOut = 1000)
        {
            int time = 0;
            while (!tcp.IsConnected && time < timeOut)
            {
                time++;
                Thread.Sleep(1);
            }
        }

        public static void WaitTime(int timeOut = 1000)
        {
            int time = 0;
            while (time < timeOut)
            {
                time++;
                Thread.Sleep(1);
            }
        }

        public static void WaitValue(ref int value, int result, int timeOut = 1000)
        {
            int time = 0;
            while (value != result && time < timeOut)
            {
                time++;
                Thread.Sleep(1);
            }
        }

        /// <summary>
        /// Wait param is true
        /// </summary>
        /// <param name="args"></param>
        /// <param name="timeOut"></param>
        public static void WaitTrue(ref bool args, int timeOut = 1000)
        {
            int time = 0;
            while (!args && time < timeOut)
            {
                time++;
                Thread.Sleep(1);
            }
        }
    }
}
