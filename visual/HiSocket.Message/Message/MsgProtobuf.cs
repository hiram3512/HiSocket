//****************************************************************************
// Description:
// Author: hiramtan@live.com
//***************************************************************************
using ProtoBuf;
using System.IO;
using HiSocket;

namespace HiSocket.Message
{
    public class MsgProtobuf : MsgBase
    {
        public MsgProtobuf() : base()
        {

        }
        public MsgProtobuf(IByteArray byteArray) : base(byteArray)
        {
        }

        public void Write<T>(T t)
        {
            var bytes = Serialize(t);
            ByteArray.Write(bytes);
        }

        public T Read<T>()
        {
            return Deserialize<T>(ByteArray.Read(ByteArray.Length));
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
