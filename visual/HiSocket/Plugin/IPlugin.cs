/***************************************************************
 * Description: 
 *
 * Documents: https://github.com/hiramtan/HiSocket
 * Author: hiramtan@live.com
***************************************************************/

namespace HiSocket
{
    public interface IPlugin
    {
        string Name { get; }
        ITcpConnection Connection { get; }
    }
}
