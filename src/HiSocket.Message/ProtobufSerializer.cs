/***************************************************************
 * Description: 
 *
 * Documents: https://github.com/hiramtan/HiSocket
 * Author: hiramtan@live.com
***************************************************************/

using System;
using ProtoBuf;
using System.IO;

namespace HiSocket.Message
{
    public static class ProtobufSerializer
    {
        public static byte[] Serialize<T>(T t)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                Serializer.Serialize<T>(stream, t);
                return stream.ToArray();
            }
        }

        public static T Deserialize<T>(byte[] param)
        {
            using (MemoryStream stream = new MemoryStream(param))
            {
                T obj = default(T);
                obj = Serializer.Deserialize<T>(stream);
                return obj;
            }
        }

        public static byte[] Serialize(object obj)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                Serializer.NonGeneric.Serialize(stream, obj);
                return stream.ToArray();
            }
        }

        public static object Deserialize(Type type, byte[] bytes)
        {
            using (MemoryStream stream = new MemoryStream(bytes))
            {
                var obj = Serializer.NonGeneric.Deserialize(type, stream);
                return obj;
            }
        }
    }
}