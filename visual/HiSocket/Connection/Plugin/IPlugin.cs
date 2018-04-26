/***************************************************************
 * Description: 
 *
 * Documents: https://github.com/hiramtan/HiSocket_unity
 * Author: hiramtan@live.com
***************************************************************/

namespace HiSocket
{
    public interface IPlugin
    {
        string Name { get; }
        IConnection Connection { get; }
    }
}
