//****************************************************************************
// Description:
// Author: hiramtan@qq.com
//****************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HiSocket.Msg;

namespace HiSocket
{
    class Example:IPackage
    {
        IMsgRegister iMsgRegister = new MsgRegister();
        void Main()
        {
            iMsgRegister.Regist(123,OnMsg);
        }

        public void Unpack(IByteArray bytes)
        {
            //
            //
           iMsgRegister.Dispatch(123,bytes);
        }

        public void Pack(IByteArray bytes)
        {
            //
        }

        void OnMsg(IByteArray iByteArray)
        {
            Console.WriteLine("get msg");
        }
    }
}
