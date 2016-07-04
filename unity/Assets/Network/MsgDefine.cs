//*********************************************************************
// Description:
// Author: hiramtan@live.com
//*********************************************************************
using UnityEngine;
using System.Collections;

namespace HiSocket
{
    internal class MsgDefine
    {
        public const int Length = 2;//用两个字节的"无符号int"(uint16=ushort)存储包的长度(整个套接字的长度:包括长度占用的两个字节)
    }
}
