//*********************************************************************
// Description:
// Author: hiramtan@live.com
//*********************************************************************


using System;
using System.IO;
using System.Text;
using HiSocket;
using HiSocket.Protobuf;
using HiSocket.Tcp;
using HiSocket.TCP;
using ProtoBuf;


public class MsgProtobuf : MsgBase, IMsg
{
    protected string protobufName;
    protected UInt16 protobufLength;
    protected byte[] protobufBody;
    public MsgProtobuf() : base()
    {
        id = MsgDefine.protobufID;
    }
    public MsgProtobuf(byte[] param) : base(param)
    { }
    public void Flush()
    {
        Flush(MsgDefine.protobufID);
    }
    public void Write<T>(T param)
    {
        protobufName = typeof(T).FullName;
        byte[] name = GetBytes(protobufName, MsgDefine.protobufNameLength);
        list.AddRange(name);
        using (MemoryStream stream = new MemoryStream())
        {
            Serializer.Serialize<T>(stream, param);
            protobufBody = stream.ToArray();
            protobufLength = (UInt16)protobufBody.Length;
        }
        byte[] length = BitConverter.GetBytes(protobufLength);
        list.AddRange(length);
        list.AddRange(protobufBody);
    }
    public T Read<T>()
    {
        index += MsgDefine.protobufNameLength;
        int length = (int)BitConverter.ToInt16(buffer, index);
        index += MsgDefine.protobufBodyLength;
        byte[] protocol = new byte[length];
        Array.Copy(buffer, index, protocol, 0, protocol.Length);
        return Deserialize<T>(protocol);
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
    private byte[] GetBytes(string paramString, int paramCount)
    {
        byte[] bytes = new byte[paramCount];
        byte[] temp = Encoding.UTF8.GetBytes(paramString);
        temp.CopyTo(bytes, 0);
        return bytes;
    }
}

