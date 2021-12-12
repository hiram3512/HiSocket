/***************************************************************
 * Description: Block buffer for reuse array
 * 
 * Documents: https://github.com/hiram3512/HiSocket
 * Support: hiramtan@live.com
***************************************************************/

using System;
using System.Collections.Generic;
using System.Text;

namespace HiSocket.Tcp
{
    /// <summary>
    /// 
    /// </summary>
    public class TcpConnection : TcpSocket, ITcpConnection
    {
        /// <summary>
        /// Trigger when send message
        /// </summary>
        public event Action<byte[]> OnSendMessage;

        /// <summary>
        /// Trigger when recieve message
        /// </summary>
        public event Action<byte[]> OnReceiveMessage;

        private IPackage _package;

        public TcpConnection(IPackage package) : base()
        {
            if (package == null)
            {
                ExceptionEvent(new Exception("package is null"));
            }
            _package = package;
            OnReceiveBytes += OnReceiveBytesFromSocket;
        }


        public void Send(byte[] data)
        {
            if (data == null || data.Length == 0)
            {
                ExceptionEvent(new Exception("data null or empty"));
            }
            SendMessageEvent(data);
            _package.Pack(data, SendBuffer);
            SendBytesInBuffer();
        }

        private void OnReceiveBytesFromSocket(IBlockBuffer<byte> buffer)
        {
            byte[] bytes = new byte[] { };
            _package.Unpack(buffer, ref bytes);
            ReceiveMessageEvent(bytes);
        }

        private void ReceiveMessageEvent(byte[] data)
        {
            if (OnReceiveMessage != null)
            {
                OnReceiveMessage(data);
            }
        }
        private void SendMessageEvent(byte[] data)
        {
            if (OnSendMessage != null)
            {
                OnSendMessage(data);
            }
        }
    }
}
