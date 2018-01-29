using HiSocket;
using NUnit.Framework;

namespace NUnit.Tests
{
    [TestFixture]
    public class TestUdp
    {
        [Test]
        public void TestMethod()
        {
            // TODO: Add your test code here
            Assert.Pass("Your first passing test");
        }

        private UdpConnection udp;
        void Start()
        {
            udp = new UdpConnection();
            udp.Connect("", 7777);

            udp.StateChangeHandler = OnStateChange;
            udp.ReceiveHandler = OnReceive;

            udp.DisConnect();
        }

        void Update()
        {
            udp.Run();
        }

        void OnStateChange(SocketState state)
        {

        }

        void OnReceive(byte[] bytes)
        {

        }
    }
}
