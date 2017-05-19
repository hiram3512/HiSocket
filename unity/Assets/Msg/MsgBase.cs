//*********************************************************************
// Description:
// Author: hiramtan@live.com
//*********************************************************************

using System;
using System.Collections.Generic;

namespace HiSocket
{
    public abstract class MsgBase : IMsg
    {
        public UInt16 length { get; private set; }

        protected byte[] buffer;
        protected List<byte> list = new List<byte>();
        /// <summary>
        /// 用于创建消息
        /// </summary>
        protected MsgBase()
        {
        }
        /// <summary>
        /// 用于解析消息
        /// </summary>
        /// <param name="param"></param>
        public MsgBase(byte[] param)
        {
            buffer = param;
        }
        public virtual void Flush()
        {
            byte[] temp = BitConverter.GetBytes(length);
            list.InsertRange(0, temp);
            MsgManager.Instance.SendMsg(buffer);
        }
    }
}