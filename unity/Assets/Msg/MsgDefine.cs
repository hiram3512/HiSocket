//*********************************************************************
// Description:
// Author: hiramtan@live.com
//*********************************************************************
using UnityEngine;
using System.Collections;

namespace HiSocket.Tcp
{

    public class MsgDefine
    {
        /// <summary>
        /// 使用uint标识消息的长度(整个套接字的长度)
        /// </summary>
        public static uint length = 0;

        /// <summary>
        /// 使用ushort标识消息协议
        /// </summary>
        public static ushort protocalId = 0;
    }
}

