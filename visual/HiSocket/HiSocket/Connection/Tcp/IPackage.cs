//****************************************************************************
// Description:处理拆包粘包
// Author: hiramtan@live.com
//***************************************************************************



namespace HiSocket
{
    public interface IPackage
    {
        void Unpack(IByteArray bytes);
        void Pack(IByteArray bytes);
    }
}
