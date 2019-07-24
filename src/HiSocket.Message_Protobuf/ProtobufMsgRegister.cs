//****************************************************************************
// Description:
// Author: hiramtan@live.com
//***************************************************************************
using HiFramework.Assert;
using HiSocket.Message;
using System;
using System.Collections.Generic;

namespace HiSocket
{
    public class ProtobufMsgRegister
    {
        private static readonly object _locker = new object();

        private static readonly Dictionary<string, Action<byte[]>> _msgs = new Dictionary<string, Action<byte[]>>();

        public static void Init()
        {
            MsgRegistHelper.Init();
        }

        public static void Regist(string key, Action<byte[]> onMsg)
        {
            lock (_locker)
            {
                AssertThat.IsFalse(_msgs.ContainsKey(key), "Already regist this key:" + key);
                _msgs.Add(key, onMsg);
            }
        }

        public static void Regist(int id, Action<byte[]> onMsg)
        {
            lock (_locker)
            {
                var key = id.ToString();
                AssertThat.IsFalse(_msgs.ContainsKey(key), "Already regist this key:" + key);
                _msgs.Add(key, onMsg);
            }
        }

        public static void Unregist(string key)
        {
            lock (_locker)
            {
                AssertThat.IsTrue(_msgs.ContainsKey(key));
                _msgs.Remove(key);
            }
        }

        public static void Unregist(int id)
        {
            lock (_locker)
            {
                var key = id.ToString();
                AssertThat.IsTrue(_msgs.ContainsKey(key));
                _msgs.Remove(key);
            }
        }

        public static void Dispatch(string key, byte[] obj)
        {
            lock (_locker)
            {
                AssertThat.IsTrue(_msgs.ContainsKey(key));
                _msgs[key](obj);
            }
        }
    }
}
