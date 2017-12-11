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
        private readonly Dictionary<int, Action<IByteArray>> _msgDic = new Dictionary<int, Action<IByteArray>>();

        private readonly object _locker = new object();

        public void Regist(int id, Action<IByteArray> action)
        {
            lock (_locker)
            {
                try
                {
                    _msgDic.Add(id, action);
                }
                catch (Exception e)
                {
                    throw new Exception(e.ToString());
                }
            }
        }

        public void Unregist(int id)
        {
            lock (_locker)
            {
                try
                {
                    _msgDic.Remove(id);
                }
                catch (Exception e)
                {
                    throw new Exception(e.ToString());
                }
            }
        }

        public void Dispatch(int id, IByteArray iByteArray)
        {
            lock (_locker)
            {
                try
                {
                    _msgDic[id](iByteArray);
                }
                catch (Exception e)
                {
                    throw new Exception(e.ToString());
                }
            }
        }
    }
}
