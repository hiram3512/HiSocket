//*********************************************************************
// Description:
// Author: hiramtan@live.com
//*********************************************************************

using System;
using System.Collections.Generic;
using HiSocket.Tcp;

namespace HiSocket
{
    public abstract class MsgBase
    {
        private UInt16 length;
        private UInt16 flag = 0;
        protected UInt16 id = 0;
        private UInt32 order = 0;
        private UInt32 time = 0;
        protected int index;
        protected byte[] buffer;
        protected List<byte> list;
        protected MsgBase()
        {
            list = new List<byte>();
        }
        public MsgBase(byte[] param)
        {
            buffer = param;
            index = MsgDefine.length + MsgDefine.flag + MsgDefine.id + MsgDefine.order + MsgDefine.time;
        }
        protected void Flush(UInt16 param)
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
            byte[] b3 = BitConverter.GetBytes(id);
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