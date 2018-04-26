/***************************************************************
 * Description: 
 *
 * Documents: https://github.com/hiramtan/HiSocket_unity
 * Author: hiramtan@live.com
***************************************************************/

namespace HiSocket
{
    class UdpConection : ConnectionBase
    {
        public UdpConection() : base(new UdpSocket())
        {
        }
    }
}
