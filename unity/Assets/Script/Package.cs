//****************************************************************************
// Description:
// Author: hiramtan@qq.com
//***************************************************************************

using System;
using HiSocket;

public class Package : HiSocket.IPackage
{
    public void Unpack(IByteArray bytes)
    {
        //解包:粘包处理
        throw new System.NotImplementedException();

        if (bytes.Length > 2)
        {
            var t = bytes.Read(2);
            BitConverter.ToInt16(t, 0);
            //isgethead = true
        }

    }

    public void Pack(IByteArray bytes)
    {
        //封包
        throw new System.NotImplementedException();
    }
}