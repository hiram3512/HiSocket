//*********************************************************************
// Description:
// Author: hiramtan@live.com
//*********************************************************************

using System;

namespace HiSocket
{
    public interface IByteMsg : IMsg
    {
        /// <summary>
        /// 使用无符号16位int标识消息协议
        /// </summary>
        UInt16 protocal { get; }
    }
}
