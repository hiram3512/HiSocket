//****************************************************************************
// Description:
// Author: hiramtan@qq.com
//***************************************************************************

using System.IO;

namespace HiSocket
{
    interface IProto
    {
        void Unpack(MemoryStream ms);
        void Pack(MemoryStream ms);

    }
}
