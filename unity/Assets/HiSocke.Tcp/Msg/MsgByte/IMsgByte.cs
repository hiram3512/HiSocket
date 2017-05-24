//*********************************************************************
// Description:
// wire protocol is a list of :   body
//                                               (bytes)
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
