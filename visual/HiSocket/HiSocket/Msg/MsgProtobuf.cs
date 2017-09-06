//****************************************************************************
// Description:
// Author: hiramtan@qq.com
//***************************************************************************
using ProtoBuf;
using System.IO;

namespace HiSocket.Msg
{
    class MsgProtobuf : MsgBase
    {
        public MsgProtobuf(IByteArray iByteArray) : base(iByteArray)
        {
        }

        private byte[] Serialize<T>(T t)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                Serializer.Serialize<T>(stream, t);
                return stream.ToArray();
            }
        }
        private T Deserialize<T>(byte[] param)
        {
            using (MemoryStream stream = new MemoryStream(param))
            {
                T obj = default(T);
                obj = Serializer.Deserialize<T>(stream);
                return obj;
            }
        }
    }
}
