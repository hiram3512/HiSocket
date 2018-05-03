/***************************************************************
 * Description: 
 *
 * Documents: https://github.com/hiramtan/HiSocket_unity
 * Author: hiramtan@live.com
***************************************************************/

namespace HiSocket
{
    public class TcpConnection : ConnectionBase, ITcp
    {
        private IPackage _iPackage;
        private readonly IByteArray _send = new ByteArray();
        private readonly IByteArray _receive = new ByteArray();
        public TcpConnection(IPackage package) : base(new TcpSocket())
        {
            _iPackage = package;
        }

        public override void Send(byte[] bytes)
        {
            _send.Write(bytes);
            _iPackage.Pack(_send, x => { base.Send(x); });
        }

        protected override void OnReceiveFromSocket(byte[] bytes)
        {
            _receive.Write(bytes);
            _iPackage.Unpack(_receive, x => { base.OnReceiveFromSocket(x); });
        }

        public bool IsConnected
        {
            get { return (ISocket as ITcp).IsConnected; }
        }
    }
}
