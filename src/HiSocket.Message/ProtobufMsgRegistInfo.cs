//****************************************************************************
// Description:
// Author: hiramtan@live.com
//***************************************************************************

using System;

namespace HiSocket.Message
{
    public class ProtobufMsgRegistInfo<T> : IProtobufMsgRegistInfo where T : class
    {
        private Action<T> _onAction;

        public ProtobufMsgRegistInfo(Action<T> onAction)
        {
            _onAction = onAction;
        }

        public void OnBytes(byte[] msg)
        {
            var type = typeof(T);
            var obj = ProtobufSerializer.Deserialize(type, msg) as T;
            _onAction.Invoke(obj);
        }
    }
}
