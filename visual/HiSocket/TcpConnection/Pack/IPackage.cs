/***************************************************************
 * Description: pack and unpack message
 *
 * Documents: https://github.com/hiramtan/HiSocket
 * Author: hiramtan@live.com
***************************************************************/

using System;

namespace HiSocket
{
    public interface IPackage
    {
        /// <summary>
        /// 在此处理接收到服务器数据后的拆包粘包
        /// </summary>
        /// <param name="bytes"></param>
        void Unpack(IByteArray source, Action<byte[]> unpackedHandler);

        /// <summary>
        /// 在此处理将要发送的数据添加长度消息id等
        /// </summary>
        /// <param name="bytes"></param>
        void Pack(IByteArray source, Action<byte[]> packedHandler);
    }
}
