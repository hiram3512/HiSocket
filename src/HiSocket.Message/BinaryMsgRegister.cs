//****************************************************************************
// Description:
// Author: hiramtan@live.com
//***************************************************************************

using HiFramework;
using System;
using System.Collections.Generic;
using System.IO;

namespace HiSocket.Message
{
    /// <summary>
    /// Regist binary message with id(int)
    /// </summary>
    public static class BinaryMsgRegister
    {
        private static readonly Dictionary<int, Action<BinaryReader>> _msgs = new Dictionary<int, Action<BinaryReader>>();

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
        public static void Regist(int key, Action<BinaryReader> onMsg)
        {
            AssertThat.IsFalse(_msgs.ContainsKey(key), "Already regist this key:" + key);
            _msgs.Add(key, onMsg);
        }

        /// <summary>
        /// Unregist by user
        /// </summary>
        /// <param name="key"></param>
        public static void Unregist(int key)
        {
            AssertThat.IsTrue(_msgs.ContainsKey(key));
            _msgs.Remove(key);
        }

        /// <summary>
        /// Dispatch message from socket
        /// </summary>
        /// <param name="key"></param>
        /// <param name="bytes"></param>
        public static void Dispatch(int key, byte[] bytes)
        {
            AssertThat.IsTrue(_msgs.ContainsKey(key));
            var ms = new MemoryStream(bytes);
            var reader = new BinaryReader(ms);
            _msgs[key](reader);
        }
    }
}
