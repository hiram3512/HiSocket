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

        protected int index;
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
            index += sizeof(UInt16);//接口IMsg中Length占用字节长度
        }
        public virtual void Flush()
        {
            int size = MsgDefine.length + MsgDefine.flag + MsgDefine.id +
                       MsgDefine.order + MsgDefine.time + list.Count;
            buffer = new byte[size];
            length = (UInt16)(size - MsgDefine.length - MsgDefine.flag);
            int index = 0;
            byte[] b1 = BitConverter.GetBytes(length);
            b1.CopyTo(buffer, index);
            index += MsgDefine.length;
            byte[] b2 = BitConverter.GetBytes(flag);
            b2.CopyTo(buffer, index);
            index += MsgDefine.flag;
            byte[] b3 = BitConverter.GetBytes(protoID);
            b3.CopyTo(buffer, index);
            index += MsgDefine.id;
            byte[] b4 = BitConverter.GetBytes(order);
            b4.CopyTo(buffer, index);
            index += MsgDefine.order;
            byte[] b5 = BitConverter.GetBytes(time);
            b5.CopyTo(buffer, index);
            index += MsgDefine.time;
            list.CopyTo(buffer, index);
            MsgManager.Instance.SendMsg(buffer);
        }
    }
}