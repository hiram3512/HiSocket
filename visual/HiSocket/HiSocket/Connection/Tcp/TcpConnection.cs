/***************************************************************
 * Description: 
 *
 * Documents: https://github.com/hiramtan/HiSocket_unity
 * Author: hiramtan@live.com
***************************************************************/

namespace HiSocket
{
    class TcpConnection : ConnectionBase
    {
        private IPackage _iPackage;
        private IByteArray _writer = new ByteArray();
        private IByteArray _reader = new ByteArray();
        public TcpConnection(IPackage package) : base(new TcpSocket())
        {
            _iPackage = package;
        }

        public override void Send(byte[] bytes)
        {
            _iPackage.Pack(bytes, _writer);
            base.Send(_writer.Read(_writer.Length));
        }

        protected override void OnReceiveFromSocket(byte[] bytes)
        {
            _reader.Write(bytes);
            _iPackage.Unpack(_reader, bytes);
            base.OnReceiveFromSocket(bytes);
        }
    }
}
