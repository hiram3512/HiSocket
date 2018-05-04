/***************************************************************
 * Description: 
 *
 * Documents: https://github.com/hiramtan/HiSocket
 * Author: hiramtan@live.com
***************************************************************/

namespace HiSocket.Tcp
{
    public interface IPlugin
    {
        string Name { get; }
        ITcpConnection Connection { get; }
    }
}
