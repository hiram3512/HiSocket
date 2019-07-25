using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HiSocket.Message;

namespace HiSocket.Example
{
    class ProtobufMsg:ProtobufMsgBase
    {
        public override void Regist()
        {
            ProtobufMsgRegister.Regist<ThisIsAProtobufClass>(OnMsg);
        }

        public void OnMsg(ThisIsAProtobufClass msg)
        {
            var hp = msg.Hp;
            var attack = msg.Attack;
        }
    }

    class ThisIsAProtobufClass
    {
        public int Hp;
        public int Attack;
    }
}
