//****************************************************************************
// Description:
// Author: hiramtan@live.com
//***************************************************************************
using HiFramework;
using System;
using System.Collections.Generic;

namespace HiSocket.Message
{
    /// <summary>
    /// Regist protobuf message by class full name
    /// </summary>
    public static class ProtobufMsgRegister
    {
        private static readonly Dictionary<string, IProtobufMsgRegistInfo> _msgs = new Dictionary<string, IProtobufMsgRegistInfo>();

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
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="onMsg"></param>
        public static void Regist<T>(Action<T> onMsg) where T : class
        {
            var key = typeof(T).FullName;
            AssertThat.IsFalse(_msgs.ContainsKey(key), "Already regist this key:" + key);
            var info = new ProtobufMsgRegistInfo<T>(onMsg);
            _msgs.Add(key, info);
        }
        /// <summary>
        /// Unregist by user
        /// </summary>
        /// <param name="key"></param>
        /// <param name="key"></param>
        /// <param name="key"></param>
        public static void Unregist(Type type)
        {
            var key = type.FullName;
            AssertThat.IsTrue(_msgs.ContainsKey(key));
            _msgs.Remove(key);
        }

        /// <summary>
        /// Dispatch message from socket
        /// </summary>
        /// <param name="key"></param>
        /// <param name="bytes"></param>
        public static void Dispatch(string key, byte[] bytes)
        {
            AssertThat.IsNotNullOrEmpty(key);
            AssertThat.IsTrue(_msgs.ContainsKey(key));
            _msgs[key].OnBytes(bytes);
        }
    }
}
