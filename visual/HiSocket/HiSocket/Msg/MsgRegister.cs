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
    public class MsgRegister : IMsgRegister
    {
        Dictionary<int, Action<byte[]>> _msgDic = new Dictionary<int, Action<byte[]>>();

        public void Regist(int id, Action<byte[]> action)
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

        public void Dispatch(int id, byte[] iProtobuf)
        {
            if (!_msgDic.ContainsKey(id))
            {
                throw new Exception("should regist first:" + id);
            }
            _msgDic[id](iProtobuf);
        }
    }
}
