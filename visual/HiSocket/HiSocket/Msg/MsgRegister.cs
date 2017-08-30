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
        private Dictionary<int, Action<byte[]>> _msgDic = new Dictionary<int, Action<byte[]>>();

        private readonly object _locker = new object();

        public void Regist(int id, Action<byte[]> action)
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

        public void Dispatch(int id, byte[] iProtobuf)
        {
            lock (_locker)
            {
                if (!_msgDic.ContainsKey(id))
                {
                    throw new Exception("should regist first:" + id);
                }
                _msgDic[id](iProtobuf);
            }
        }
    }
}
