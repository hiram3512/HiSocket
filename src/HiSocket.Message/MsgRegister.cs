//****************************************************************************
// Description:
// why key's type is string? because some project not only use id(int) regist and also protobuf's name
// Author: hiramtan@live.com
//***************************************************************************
using System;
using System.Collections.Generic;
using HiSocket;

namespace HiSocket.Message
{
    public static class MsgRegister
    {
        private static readonly Dictionary<string, Action<IByteArray>> msgs = new Dictionary<string, Action<IByteArray>>();

        private static readonly object locker = new object();

        public static void Regist(string key, Action<IByteArray> action)
        {
            lock (locker)
            {
                msgs.Add(key, action);
            }
        }

        public static void Unregist(string key)
        {
            lock (locker)
            {
                msgs.Remove(key);
            }
        }

        public static void Dispatch(string key, IByteArray iByteArray)
        {
            lock (locker)
            {
                msgs[key](iByteArray);
            }
        }

        public static bool IsContain(string key)
        {
            lock (locker)
            {
                return msgs.ContainsKey(key);
            }
        }
    }
}
