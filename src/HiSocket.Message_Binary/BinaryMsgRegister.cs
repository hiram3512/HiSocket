//****************************************************************************
// Description:
// Author: hiramtan@live.com
//***************************************************************************
using HiFramework.Assert;
using System;
using System.Collections.Generic;
using HiFramework;
using HiSocket.Message;

namespace HiSocket
{
    public class BinaryMsgRegister
    {
        private static readonly object _locker = new object();

        private static readonly Dictionary<int, Action<byte[]>> _msgs = new Dictionary<int, Action<byte[]>>();

        public static void Init()
        {
            MsgRegistHelper.Init();
        }

        public static void Regist(int key, Action<byte[]> onMsg)
        {
            lock (_locker)
            {
                AssertThat.IsFalse(_msgs.ContainsKey(key), "Already regist this key:" + key);
                _msgs.Add(key, onMsg);
            }
        }

        public static void Unregist(int key)
        {
            lock (_locker)
            {
                AssertThat.IsTrue(_msgs.ContainsKey(key));
                _msgs.Remove(key);
            }
        }

        public static void Dispatch(int key, byte[] bytes)
        {
            lock (_locker)
            {
                AssertThat.IsTrue(_msgs.ContainsKey(key));
                _msgs[key](bytes);
            }
        }
    }
}
