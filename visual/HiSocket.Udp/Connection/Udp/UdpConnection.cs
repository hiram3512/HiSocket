/***************************************************************
 * Description: 
 *
 * Documents: https://github.com/hiramtan/HiSocket_unity
 * Author: hiramtan@live.com
***************************************************************/

namespace HiSocket
{
    public class UdpConnection : ConnectionBase, IUdp
    {
        public UdpConnection(int bufferSize = 2048) : base(new UdpSocket(bufferSize))
        {
        }

        public int BufferSize
        {
            get { return (ISocket as IUdp).BufferSize; }
        }
    }
}
