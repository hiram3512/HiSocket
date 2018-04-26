/***************************************************************
 * Description: 
 *
 * Documents: https://github.com/hiramtan/HiSocket_unity
 * Author: hiramtan@live.com
***************************************************************/

namespace HiSocket
{
    /// <summary>
    /// For example
    /// </summary>
    class CalculateData : PluginBase
    {
        private int _howManyBytesSend;

        public CalculateData(string name, IConnection connection) : base(name, connection)
        {
            connection.OnSend += x => { _howManyBytesSend += x.Length; };
        }

        //class Test
        //{
        //    void Start()
        //    {
        //        var tcp = new TcpConnection(iPackage);
        //        var plugin = new CalculateData("CalculateData", tcp);
        //    }
        //}
    }
}