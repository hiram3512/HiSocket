﻿//****************************************************************************
// Description:
// Author: hiramtan@qq.com
//***************************************************************************
using System;

namespace HiSocket.Msg
{
    interface IMsgRegister
    {
        void Regist(int id, Action<byte[]> action);

        void Unregist(int id);

        void Dispatch(int id, byte[] bytes);
    }
}