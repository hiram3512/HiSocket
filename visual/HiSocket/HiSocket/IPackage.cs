//****************************************************************************
// Description:
// Author: hiramtan@qq.com
//***************************************************************************

using System.IO;

namespace HiSocket
{
    public interface IPackage
    {
        void Unpack(IByteArray bytes);
        void Pack(IByteArray bytes);
    }
}
