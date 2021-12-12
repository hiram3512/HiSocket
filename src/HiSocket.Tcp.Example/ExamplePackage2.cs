/***************************************************************
 * Description: Block buffer for reuse array
 * 
 * Documents: https://github.com/hiram3512/HiSocket
 * Support: hiramtan@live.com
***************************************************************/

using System;
using System.Collections.Generic;
using System.Text;

namespace HiSocket.Tcp.Example
{
    public class ExamplePackage2 : IPackage
    {
        public void Pack(byte[] message, IBlockBuffer<byte> sendBuffer)
        {
            //消息头,假设用int标识,4字节
            int length = message.Length;
            byte[] lengthBytes = BitConverter.GetBytes(length);
            sendBuffer.WriteAtEnd(lengthBytes);
            sendBuffer.WriteAtEnd(message);
        }
        public void Unpack(IBlockBuffer<byte> receiveBuffer, ref byte[] message)
        {
            //消息头,假设用int标识,4字节
            if (receiveBuffer.Index >= 4)
            {
                byte[] lengthBytes = receiveBuffer.TryReadFromHead(4);
                int length = BitConverter.ToInt32(lengthBytes, 0);
                if (receiveBuffer.Index >= length)//buffer中有足够数据
                {
                    receiveBuffer.IncreaseIndex(4);//消息头,假设用int标识,4字节,开始读取消息体
                    message = receiveBuffer.ReadFromHead(length);
                }
            }
        }
    }
}
