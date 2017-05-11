//*********************************************************************
// Description:
// Author: hiramtan@live.com
//*********************************************************************
using ProtoBuf;
using System.IO;

namespace HiSocket.Protobuf
{
    internal class Protobuf : Singleton<Protobuf>, IProtobuf
    {
        public byte[] Serialize<T>(T param)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                Serializer.Serialize(stream, param);
                return stream.ToArray();
            }
        }

        public T Deserialize<T>(byte[] param)
        {
            using (MemoryStream stream = new MemoryStream(param))
            {
                T t = Serializer.Deserialize<T>(stream);
                return t;
            }
        }
    }
}