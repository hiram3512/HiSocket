//****************************************************************************
// Description:
// Author: hiramtan@live.com
//***************************************************************************
using System;
using System.Collections.Generic;

namespace HiSocket
{
    public class MsgRegister : IMsgRegister
    {
        private readonly Dictionary<string, Action<IByteArray>> _msgDic = new Dictionary<string, Action<IByteArray>>();

        private readonly object _locker = new object();

        public void Regist(string key, Action<IByteArray> action)
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

        public void Unregist(string key)
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

        public void Dispatch(string key, IByteArray iByteArray)
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

        public bool IsContain(string key)
        {
            lock (_locker)
            {
                return _msgDic.ContainsKey(key);
            }
        }
    }
}
