//****************************************************************************
// Description:处理拆包粘包
// Author: hiramtan@live.com
//***************************************************************************


using System.Collections.Generic;

namespace HiSocket
{
    public interface IPackage
    {
        /// <summary>
        /// 在此处理接收到服务器数据后的拆包粘包
        /// </summary>
        /// <param name="bytes"></param>
        void Unpack(IByteArray reader, out byte[] writer);

        /// <summary>
        /// 在此处理将要发送的数据添加长度消息id等
        /// </summary>
        /// <param name="bytes"></param>
        void Pack(ref byte[] reader, IByteArray writer);
    }
}
