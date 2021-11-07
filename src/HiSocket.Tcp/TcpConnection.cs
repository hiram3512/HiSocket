using System;
using System.Collections.Generic;
using System.Text;

namespace HiSocket.Tcp
{
    class TcpConnection : TcpSocket, ITcpConnection
    {
        /// <summary>
        /// Trigger when send message
        /// </summary>
        public event Action<byte[]> OnSend;

        /// <summary>
        /// Trigger when recieve message
        /// </summary>
        public event Action<byte[]> OnReceive;


        private IPackage _package;
        public TcpConnection(IPackage package)
        {
            if (package == null)
            {
                throw new ArgumentNullException("package is null");
            }
            _package = package;
        }

        public void Send(byte[] data)
        {
            if (data == null || data.Length == 0)
            {
                throw new ArgumentException("data error");
            }
            _package.Pack(data, SendBuffer);
            SendBytes(SendBuffer.Buffer, 0, SendBuffer.Index);
        }
    }
}
