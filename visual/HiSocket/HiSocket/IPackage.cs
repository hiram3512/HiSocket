//****************************************************************************
// Description:
// Author: hiramtan@qq.com
//***************************************************************************

using System.IO;

namespace HiSocket
{
    public interface IPackage
    {
        void Unpack(byte[] bytes);
        void Pack(byte[] bytes);

    }
}
