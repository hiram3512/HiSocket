//****************************************************************************
// Description:
// Author: hiramtan@live.com
//***************************************************************************
using HiFramework.Assert;
using System;
using System.Collections.Generic;

namespace HiSocket.Message
{
    /// <summary>
    /// Regist protobuf message by class full name
    /// </summary>
    public static class ProtobufMsgRegister
    {
        private static readonly object _locker = new object();

        private static readonly Dictionary<string, Action<object>> _msgs = new Dictionary<string, Action<object>>();

        /// <summary>
        /// This can quick regist all message
        /// </summary>
        public static void Init()
        {
            BinaryRegistHelper.Init();
        }

        /// <summary>
        /// Regist message by user
        /// </summary>
        /// <param name="key"></param>
        /// <param name="onMsg"></param>
        public static void Regist(Type type, Action<object> onMsg)
        {
            lock (_locker)
            {
                var key = type.FullName;
                AssertThat.IsFalse(_msgs.ContainsKey(key), "Already regist this key:" + key);
                _msgs.Add(key, onMsg);
            }
        }

        /// <summary>
        /// Unregist by user
        /// </summary>
        /// <param name="key"></param>
        /// <param name="key"></param>
        /// <param name="key"></param>
        public static void Unregist(Type type)
        {
            lock (_locker)
            {
                var key = type.FullName;
                AssertThat.IsTrue(_msgs.ContainsKey(key));
                _msgs.Remove(key);
            }
        }

        /// <summary>
        /// Dispatch message from socket
        /// </summary>
        /// <param name="key"></param>
        /// <param name="bytes"></param>
        public static void Dispatch(string key, byte[] bytes)
        {
            lock (_locker)
            {
                AssertThat.IsNotNullOrEmpty(key);
                AssertThat.IsTrue(_msgs.ContainsKey(key));
                var type = Type.GetType(key);
                var obj = ProtobufSerializer.Deserialize(type, bytes);
                _msgs[key](obj);
            }
        }
    }
}
