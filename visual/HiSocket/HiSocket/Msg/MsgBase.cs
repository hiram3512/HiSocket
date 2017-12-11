//****************************************************************************
// Description:
// Author: hiramtan@live.com
//***************************************************************************

namespace HiSocket.Msg
{
    public abstract class MsgBase
    {
        public readonly IByteArray _iByteArray;

        protected MsgBase(IByteArray iByteArray)
        {
            _iByteArray = iByteArray;
        }
    }
}