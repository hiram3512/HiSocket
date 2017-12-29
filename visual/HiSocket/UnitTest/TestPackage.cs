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
            throw new NotImplementedException();
        }

        public void Pack(IByteArray bytes)
        {
            throw new NotImplementedException();
        }
    }
}
