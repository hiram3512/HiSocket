//****************************************************************************
// Description:
// Author: hiramtan@qq.com
//***************************************************************************

using HiSocket;

public class Package : HiSocket.IPackage
{
    public void Unpack(IByteArray bytes)
    {
        //解包:粘包处理
        throw new System.NotImplementedException();
    }

    public void Pack(IByteArray bytes)
    {
        //封包
        throw new System.NotImplementedException();
    }
}