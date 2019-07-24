using HiSocket.Message;
using System.IO;

namespace HiSocket
{
    public abstract class BinaryMsgBase : MsgRegistHelper
    {
        protected BinaryReader BinaryReader;

        protected BinaryWriter BinaryWriter;

        private MemoryStream _msReader;
        private MemoryStream _msWriter;

        /// <summary>
        /// for pack
        /// </summary>
        public BinaryMsgBase()
        {
            _msReader = new MemoryStream();
            BinaryWriter = new BinaryWriter(_msReader);
        }

        /// <summary>
        /// for unpack
        /// </summary>
        /// <param name="bytes"></param>
        public BinaryMsgBase(byte[] bytes)
        {
            _msWriter = new MemoryStream(bytes);
            BinaryReader = new BinaryReader(_msWriter);
        }

        protected void Flush()
        {
            var bytes = _msWriter.GetBuffer();

        }
    }
}
