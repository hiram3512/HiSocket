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
}