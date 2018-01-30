//****************************************************************************
// Description:regist delegate
// why key's type is string? because some project not only use id regist and also protobuf's name
// Author: hiramtan@live.com
//***************************************************************************
using System;

namespace HiSocket
{
    public interface IMsgRegister
    {
        void Regist(string key, Action<IByteArray> action);

        void Unregist(string key);

        void Dispatch(string key, IByteArray iByteArray);

        bool IsContain(string key);
    }
}
