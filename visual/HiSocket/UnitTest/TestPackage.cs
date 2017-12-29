using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HiSocket;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class TestPackage:IPackage
    {
        public void Unpack(IByteArray bytes)
        {
            MsgProtobuf msg = new MsgProtobuf(bytes);
            msg.Read<int>();
        }

        public void Pack(IByteArray bytes)
        {
            bytes.Insert(0,new byte[]{1});
        }
    }
}
