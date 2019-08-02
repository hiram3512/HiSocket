/***************************************************************
 * Description: 
 *
 * Documents: https://github.com/hiramtan/HiSocket
 * Author: hiramtan@live.com
***************************************************************/
using HiSocket.Tcp;

namespace HiSocket.Example
{
    /// <summary>
    /// The recommend is use TcpConnection 
    /// </summary>
    class Example3
    {
        TcpSocket tcp; //The recommend is use TcpConnection 
        void Connect()
        {
            tcp = new TcpSocket(1024);//set buffer size
            tcp.OnReceiveBytes += OnReceive;
            tcp.Connect("127.0.0.1", 999);
        }

        void OnReceive(byte[] bytes)
        {
            //split bytes here
        }
    }
}
