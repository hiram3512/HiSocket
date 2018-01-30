using NUnit.Framework;
using HiSocket;

namespace NUnit.Tests
{
    [TestFixture]
    public class TestByteMsg
    {
        [Test]
        public void TestMethod()
        {
            // TODO: Add your test code here
            Assert.Pass("Your first passing test");
        }

        #region GetMsg
        void Regist()
        {
            MsgRegister msgRegister = new MsgRegister();
            msgRegister.Regist("id", OnMsg);
        }

        void OnMsg(IByteArray iByteArray)
        {
            MsgBytes msgBytes = new MsgBytes(iByteArray);
            int x = msgBytes.Read<int>();
        }
        #endregion

        #region  SendMsg
        void Send()
        {
            MsgBytes msgBytes = new MsgBytes();
            int x = 10;
            msgBytes.Write(x);
            //tcp.send(msgBytes.ByteArray.Read(msgBytes.ByteArray.Length));
        }
        #endregion
    }
}
