using HiSocket;
using NUnit.Framework;
using System.Text;

namespace NUnit.Tests
{
    [TestFixture]
    public class TestByteArray
    {
        [Test]
        public void TestMethod()
        {
            IByteArray i = new ByteArray();
            Assert.AreEqual(i.Length, 0);

            byte[] hello = Encoding.UTF8.GetBytes("hello");
            byte[] world = Encoding.UTF8.GetBytes("world");

            i.Write(hello);
            Assert.AreEqual(i.Length, 5);
            var hello2 = i.Read(5);
            Assert.AreEqual(hello, hello2);
        }
    }
}
