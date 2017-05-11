using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using HiSocket;
using ProtoBuf;

namespace HiSocket.Protobuf
{
    public class test : Singleton<test>, IProtobuf
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
            throw new System.NotImplementedException();
        }
    }
}