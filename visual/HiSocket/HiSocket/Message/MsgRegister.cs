//****************************************************************************
// Description:
// why key's type is string? because some project not only use id(int) regist and also protobuf's name
// Author: hiramtan@live.com
//***************************************************************************
using System;
using System.Collections.Generic;

namespace HiSocket
{
    public static class MsgRegister
    {
        private static readonly Dictionary<string, Action<IByteArray>> _msgDic = new Dictionary<string, Action<IByteArray>>();

        private static readonly object _locker = new object();

        public static void Regist(string key, Action<IByteArray> action)
        {
            lock (_locker)
            {
                try
                {
                    _msgDic.Add(key, action);
                }
                catch (Exception e)
                {
                    throw new Exception(e.ToString());
                }
            }
        }

        public static void Unregist(string key)
        {
            lock (_locker)
            {
                try
                {
                    _msgDic.Remove(key);
                }
                catch (Exception e)
                {
                    throw new Exception(e.ToString());
                }
            }
        }

        public static void Dispatch(string key, IByteArray iByteArray)
        {
            lock (_locker)
            {
                try
                {
                    _msgDic[key](iByteArray);
                }
                catch (Exception e)
                {
                    throw new Exception(e.ToString());
                }
            }
        }

        public static bool IsContain(string key)
        {
            lock (_locker)
            {
                return _msgDic.ContainsKey(key);
            }
        }
    }
}
