//****************************************************************************
// Description:粘包拆包,封包处理
// Author: hiramtan@qq.com
//***************************************************************************

using HiSocket.Msg;
using System;

public class Package : IPackage
{
    private IMsgRegister iMsgRegister;
    public void Unpack(IByteArray bytes)
    {
        if (bytes.Length >= 2)//两字节表示消息体长度
        {
            var lengthBytes = bytes.Read(2);
            UInt16 length = BitConverter.ToUInt16(lengthBytes, 0);
            if (bytes.Length >= length) //读取消息体
            {
                int id = BitConverter.ToUInt16((bytes.Read(4)), 0);
                iMsgRegister.Dispatch(id, bytes);
            }
            else
            {
                bytes.Insert(0, lengthBytes);//重新添加,等待下次拆包
            }
        }
    }

    public void Pack(IByteArray bytes)
    {
        UInt16 length = (UInt16)bytes.Length;
        byte[] lengthBytes = BitConverter.GetBytes(length);
        bytes.Insert(0, lengthBytes);//消息头写入长度
    }
}