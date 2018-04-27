/***************************************************************
 * Description: 
 *
 * Documents: https://github.com/hiramtan/HiSocket_unity
 * Author: hiramtan@live.com
***************************************************************/

namespace HiSocket
{
    public class UdpConnection : ConnectionBase
    {
        public UdpConnection(int bufferSize = 2048) : base(new UdpSocket(bufferSize))
        {
        }
    }
}
