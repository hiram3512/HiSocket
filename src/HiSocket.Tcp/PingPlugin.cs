/***************************************************************
 * Description:   
 * bug: if you use this logic with unity(.net3.5)will occur a bug, but unity(.net4.6)will fine.
 * 
 * Documents: https://github.com/hiramtan/HiSocket
 * Author: hiramtan@live.com
***************************************************************/

using System.Net;
using System.Net.NetworkInformation;

namespace HiSocket.Tcp
{
    public sealed class PingPlugin : PluginBase
    {
        public long Ping;
        public PingPlugin(string name) : base(name)
        {
            PingLogic();
        }

        public void Tick()
        {
            PingLogic();
        }

        void PingLogic()
        {
            var sender = new Ping();
            var ip = (TcpConnection as TcpSocket).Socket.RemoteEndPoint as IPEndPoint;
            var reply = sender.Send(ip.Address);
            if (reply.Status == IPStatus.Success)
            {
                Ping = reply.RoundtripTime;
            }
        }
    }
}
