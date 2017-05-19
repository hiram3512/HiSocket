//*********************************************************************
// Description:
// Author: hiramtan@live.com
//*********************************************************************
using System;

namespace HiSocket
{
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