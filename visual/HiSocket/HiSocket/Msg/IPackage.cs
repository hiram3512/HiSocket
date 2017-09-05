//****************************************************************************
// Description:
// Author: hiramtan@qq.com
//***************************************************************************



namespace HiSocket.Msg
{
    public interface IPackage
    {
        void Unpack(IByteArray bytes);
        void Pack(IByteArray bytes);
    }
}
