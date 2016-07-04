//*********************************************************************
// Description:
// Author: hiramtan@live.com
//*********************************************************************

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace HiSocket
{
    public class MsgManager
    {
        public delegate void MsgEventHandler(MsgBase param);
        private static Dictionary<int, MsgEventHandler> msgDic = new Dictionary<int, MsgEventHandler>();


        public static void Register(int paramKey, MsgEventHandler paramHandler)
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
            

            int tempKey = 00000000000000000000;//havent finished, will parse user's protocal


            if (msgDic.ContainsKey(tempKey))
            {
                Debug.LogWarning("dic donnt contain this key: " + tempKey + "make sure you have register it in advance");
                return;
            }
            MsgBase tempMsgBase = new MsgBase(param);
            msgDic[tempKey](tempMsgBase);
        }
    }
}
