/***************************************************************
 * Description: 
 *
 * Documents: https://github.com/hiramtan/HiSocket
 * Author: hiramtan@live.com
***************************************************************/

using System.Net;
using System.Net.NetworkInformation;

namespace HiSocket
{
    public sealed class PingPlugin : PluginBase
    {
        public long Ping;
        public PingPlugin(string name, IConnection connection) : base(name, connection)
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
            var ip = (Connection as TcpSocket).Socket.RemoteEndPoint as IPEndPoint;
            var reply = sender.Send(ip.Address);
            if (reply.Status == IPStatus.Success)
            {
                Ping = reply.RoundtripTime;
            }
        }
    }
}
