using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using HiSocket.Message;

namespace HiSocket.Example
{
    class BinaryMsg:BinaryMsgBase
    {
        public override void Regist()
        {
           BinaryMsgRegister.Regist(1001,OnMsg);
        }

        public void OnMsg(BinaryReader reader)
        {
            int hp = reader.ReadInt32();
            int attack = reader.ReadInt32();
        }

        public void Send(int hp,int attack)
        {
            Writer.Write(BitConverter.GetBytes(hp));
            Writer.Write(BitConverter.GetBytes(attack));
            Flush();
        }
    }
}
