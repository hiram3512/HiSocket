//*********************************************************************
// Description:
// Author: hiramtan@live.com
//*********************************************************************

using System;

namespace HiSocket
{
    public interface IMsg
    {
        /// <summary>
        /// 使用无符号16位int标识整个消息的长度（ushort）
        /// </summary>
        UInt16 length { get; }

        void Flush();
    }

    public interface IByteMsg : IMsg
    {
        /// <summary>
        /// 使用无符号16位int标识消息协议
        /// </summary>
        UInt16 protocal { get; }
    }

    public interface IProtobufMsg : IMsg
    {
        /// <summary>
        /// 标识类名长度
        /// </summary>
        UInt16 nameLength { get; }
        /// <summary>
        /// 类名
        /// </summary>
        string name { get; }
    }
}