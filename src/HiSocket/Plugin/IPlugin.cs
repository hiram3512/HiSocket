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
        /// <summary>
        /// Plugins name
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Plugin belong to this connection
        /// </summary>
        IConnection Connection { get; set; }
    }
}
