//*********************************************************************
// Description:
// Author: hiramtan@live.com
//*********************************************************************

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace HiSocket.TCP
{
    public class MsgManager
    {
        public static ISocket iSocket { get; private set; }
        public delegate void MsgEventHandler(IMsg param);
        private static Dictionary<uint, MsgEventHandler> msgDic = new Dictionary<uint, MsgEventHandler>();


        public static void Init(ISocket param)
        {
            iSocket = param;
        }

        /// <summary>
        /// 注册消息回调事件
        /// </summary>
        /// <param name="paramKey">协议</param>
        /// <param name="paramHandler">事件</param>
        public static void Register(uint paramKey, MsgEventHandler paramHandler)
        {
            if (msgDic.ContainsKey(paramKey))
            {
                Debug.LogWarning("dic already contain this key: " + paramKey);
                return;
            }
            msgDic.Add(paramKey, paramHandler);
        }

        public static void ReceiveMsg(byte[] param)
        {
            ushort tempKey = BitConverter.ToUInt16(param, 0);
            if (!msgDic.ContainsKey(tempKey))
            {
                Debug.LogWarning("dic donnt contain this key: " + tempKey + "make sure you have register it in advance");
                return;
            }
            Msg tempMsg = new Msg(param);
            msgDic[tempKey](tempMsg);
            //待完成:...
            //引擎tick维护接收数据
            //保证所有信息都可以更新组件
        }
    }
}
