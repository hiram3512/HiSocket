//****************************************************************************
// Description:
// Author: hiramtan@live.com
//***************************************************************************

using HiSocket;

namespace HiSocket.Message
{
    public abstract class MsgBase
    {
        public readonly IByteArray ByteArray;

        protected MsgBase(IByteArray byteArray)
        {
            ByteArray = byteArray;
        }

        protected MsgBase()
        {
            ByteArray = new ByteArray();
        }
    }
}