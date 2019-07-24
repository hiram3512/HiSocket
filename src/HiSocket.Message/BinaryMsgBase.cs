//****************************************************************************
// Description:
// Author: hiramtan@live.com
//***************************************************************************

using System.IO;

namespace HiSocket.Message
{
    public abstract class BinaryMsgBase
    {
        /// <summary>
        /// Used for sub class write different type variable
        /// </summary>
        public BinaryWriter Writer;

        private MemoryStream _ms;

        /// <summary>
        /// for pack
        /// </summary>
        public BinaryMsgBase()
        {
            Writer = new BinaryWriter(_ms);
        }

        /// <summary>
        /// User regist here
        /// </summary>
        public abstract void Regist();

        /// <summary>
        /// Write finish and send data
        /// </summary>
        protected void Flush()
        {
            var bytes = _ms.GetBuffer();
            ChannelManager.Send(bytes);
        }
    }
}
