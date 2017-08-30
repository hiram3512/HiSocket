//****************************************************************************
// Description:
// Author: hiramtan@qq.com
//***************************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HiSocket.Msg
{
    class MsgRegister : IMsgRegister
    {
        Dictionary<int, Action<IProtobuf>> _msgDic = new Dictionary<int, Action<IProtobuf>>();

        public void Regist(int id, Action<IProtobuf> action)
        {
            if (_msgDic.ContainsKey(id))
            {
                throw new Exception("do not need to regist again:" + id);
            }
            _msgDic.Add(id, action);
        }

        public void Unregist(int id)
        {
            if (!_msgDic.ContainsKey(id))
            {
                throw new Exception("should regist first:" + id);
            }
            _msgDic.Remove(id);
        }

        public void Dispatch(int id, IProtobuf iProtobuf)
        {
            if (!_msgDic.ContainsKey(id))
            {
                throw new Exception("should regist first:" + id);
            }
            _msgDic[id](iProtobuf);
        }
    }
}
