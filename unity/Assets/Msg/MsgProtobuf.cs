//*********************************************************************
// Description:
// Author: hiramtan@live.com
//*********************************************************************
using ProtoBuf;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace HiSocket
{
    public class MsgProtobuf : MsgBase, IProtobufMsg
    {
        #region  read

        public MsgProtobuf(byte[] param) : base(param)
        {

        }
        public T Read<T>()
        {
            return Deserialize<T>(buffer);
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
        #endregion

        #region write
        public ushort nameLength { get; private set; }
        public string name { get; private set; }
        public void Write<T>(T param)
        {
            name = typeof(T).FullName;
            nameLength = (ushort)Encoding.UTF8.GetBytes(name).Length;
            list = Serialize(param).ToList();
        }
        private byte[] Serialize<T>(T t)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                Serializer.Serialize<T>(stream, t);
                return stream.ToArray();
            }
        }
        public void Flush()
        {
            byte[] tempName = Encoding.UTF8.GetBytes(name);
            list.InsertRange(0, tempName);
            byte[] tempNameLength = BitConverter.GetBytes(nameLength);
            list.InsertRange(0, tempNameLength);
            base.Flush();
        }
        #endregion
    }
}


