//****************************************************************************
// Description:
// Author: hiramtan@qq.com
//***************************************************************************
using System;
using System.Collections.Generic;

namespace HiSocket.Msg
{
    public class MsgRegister : IMsgRegister
    {
        private Dictionary<int, Action<IByteArray>> _msgDic = new Dictionary<int, Action<IByteArray>>();

        private readonly object _locker = new object();

        public void Regist(int id, Action<IByteArray> action)
        {
            lock (_locker)
            {
                if (_msgDic.ContainsKey(id))
                {
                    throw new Exception("do not need to regist again:" + id);
                }
                _msgDic.Add(id, action);
            }
        }

        public void Unregist(int id)
        {
            lock (_locker)
            {
                if (!_msgDic.ContainsKey(id))
                {
                    throw new Exception("should regist first:" + id);
                }
                _msgDic.Remove(id);
            }
        }

        public void Dispatch(int id, IByteArray iByteArray)
        {
            lock (_locker)
            {
                if (!_msgDic.ContainsKey(id))
                {
                    throw new Exception("should regist first:" + id);
                }
                _msgDic[id](iByteArray);
            }
        }
    }
}
