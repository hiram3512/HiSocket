using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HiSocket;

namespace NUnit.Tests
{
    [TestFixture]
    public class TestProtobufMsg
    {
        [Test]
        public void TestMethod()
        {
            // TODO: Add your test code here
            Assert.Pass("Your first passing test");
        }

        public class Role
        {
            public int HP;
        }
        #region GetMsg
        void Regist()
        {
            MsgRegister.Regist("id", OnMsg);
        }

        void OnMsg(IByteArray iByteArray)
        {
            MsgProtobuf msgBytes = new MsgProtobuf(iByteArray);
            Role role = msgBytes.Read<Role>();
        }
        #endregion
        #region  SendMsg
        void Send()
        {
            MsgProtobuf msgBytes = new MsgProtobuf();
            Role role = new Role();
            role.HP = 10;
            msgBytes.Write(role);
            //tcp.send(msgBytes.ByteArray.Read(msgBytes.ByteArray.Length));
        }
        #endregion
    }
}
