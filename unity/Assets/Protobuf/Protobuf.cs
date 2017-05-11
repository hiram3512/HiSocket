using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace HiSocket.Protobuf
{
    public class Protobuf : IProtobuf
    {
        public byte[] Serialize<T>(T param)
        {
            throw new System.NotImplementedException();
        }

        public T Deserialize<T>(byte[] param)
        {
            throw new System.NotImplementedException();
        }
    }
}


//usingSystem.IO;
//publicclassHiProtobuf
//{
//privatestaticSerializerserializer =newSerializer();
//publicstaticbyte[] Serialize<T>(Tobj)
//{
//    using (MemoryStreamstream = newMemoryStream())
//    {
//        serializer.Serialize(stream, obj);
//        returnstream.ToArray();
//    }
//}
//publicstaticTDeserialize<T>(byte[] data)
//{
//    using(MemoryStreamstream =newMemoryStream(data))
//    {
//        Tobj =default(T);
//        obj = (T) serializer.Deserialize(stream,null,typeof(T));
//        returnobj;
//    }
//}
//}
