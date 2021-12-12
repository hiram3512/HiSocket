/***************************************************************
 * Description: Block buffer for reuse array
 * 
 * Documents: https://github.com/hiram3512/HiSocket
 * Support: hiramtan@live.com
***************************************************************/

using System.Net;
using System.Net.NetworkInformation;

namespace HiSocket.Tcp
{
    /// <summary>
    /// 
    /// </summary>
    public static class Ping
    {
        public static System.Net.NetworkInformation.Ping sender = new System.Net.NetworkInformation.Ping();
        
        /// <summary>
        /// Get ping
        /// Note: tick to get current ping
        /// </summary>
        /// <param name="tcpConnection"></param>
        /// <returns></returns>
        public static long GetPint(TcpConnection tcpConnection)
        {
            long time = 0;
            var iep = tcpConnection.Socket.RemoteEndPoint as System.Net.IPEndPoint;
            var reply = sender.Send(iep.Address);
            if (reply.Status == IPStatus.Success)
            {
                time = reply.RoundtripTime;
            }
            return time;
        }
    }
}
