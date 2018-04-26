/***************************************************************
 * Description: 
 * UDP Sockets are "connection-less", so the protocol does not know anything about whether or not the server and client are connected.
 * Documents: https://github.com/hiramtan/HiSocket_unity
 * Author: hiramtan@live.com
***************************************************************/

namespace HiSocket
{
    public interface IUdpSocket : ISocket
    {
        int BufferSize { get; set; }
    }
}