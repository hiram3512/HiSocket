//using System;
//using HiSocket;
//using Microsoft.VisualStudio.TestTools.UnitTesting;

//namespace HiSocketTest.Connection.Scoket
//{
//    [TestClass]
//    public class TestByteBlockBuffer
//    {
//        [TestMethod]
//        public void TestByteBlock()
//        {
//            var buffer = new ByteBlockBuffer();
//            var bytes = new byte[1000];
//            buffer.WriteAllBytes(bytes);
//            Assert.AreEqual(buffer.Count, 1);
//            Assert.IsTrue(buffer.IsReaderAndWriterInSameNode());
//            buffer.WriteAllBytes(bytes);
//            Assert.AreEqual(buffer.Count, 2);
//            Assert.IsFalse(buffer.IsReaderAndWriterInSameNode());
//            var read = buffer.ReadAllBytes();
//            Assert.AreEqual(read.Length, 2000);
//        }

//        [TestMethod]
//        public void TestInSameNode()
//        {
//            var buffer = new ByteBlockBuffer(10);
//            var bytes = new byte[5];
//            buffer.WriteAllBytes(bytes);
//            var read = buffer.ReadAllBytes();
//            Assert.AreEqual(read.Length, 5);
//        }
//        [TestMethod]
//        public void TestInDiffrentNode()
//        {
//            var buffer = new ByteBlockBuffer(10);
//            var bytes = new byte[35];
//            buffer.WriteAllBytes(bytes);
//            var read = buffer.ReadAllBytes();
//            Assert.AreEqual(read.Length, 35);
//        }
//        [TestMethod]
//        public void TestInDiffrentNodeAndReuse()
//        {
//            var buffer = new ByteBlockBuffer(10);
//            var bytes = new byte[55];
//            buffer.WriteAllBytes(bytes);
//            Assert.AreEqual(buffer.Count, 6);
//            buffer.Reader.MovePosition(10);
//            buffer.Reader.MovePosition(10);
//            buffer.Reader.MovePosition(5);
//            buffer.WriteAllBytes(new byte[10]);
//            Assert.AreEqual(buffer.Count, 6);
//            buffer.WriteAllBytes(new byte[10]);
//            Assert.AreEqual(buffer.Count, 6);
//            var read = buffer.ReadAllBytes();
//            Assert.AreEqual(read.Length, 50);
//        }
//    }
//}
