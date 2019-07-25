//****************************************************************************
// Description:
// Author: hiramtan@live.com
//***************************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HiSocket.Message
{
    public interface IProtobufMsgRegistInfo
    {
        void OnBytes(byte[] msg);
    }
}
