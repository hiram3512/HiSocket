//*********************************************************************
// Description:
// Author: hiramtan@live.com
//*********************************************************************
using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace HiSocket.Tcp
{
    public class MsgBase : IMsg
    {
        private List<byte> writeBytesList = new List<byte>();

        public MsgBase()
        {
            writeBytesList = new List<byte>();
        }


        public void Write<T>(T paramValue)
        {
            if (paramValue is bool)
            {
                bool tempVelue = (bool)Convert.ChangeType(paramValue, typeof(bool));
                var tempBytes = BitConverter.GetBytes(tempVelue);
                writeBytesList.AddRange(tempBytes);
            }
            else if (paramValue is char)
            {
                char tempVelue = (char)Convert.ChangeType(paramValue, typeof(char));
                var tempBytes = BitConverter.GetBytes(tempVelue);
                writeBytesList.AddRange(tempBytes);
            }
            else if (paramValue is double)
            {
                double tempVelue = (double)Convert.ChangeType(paramValue, typeof(double));
                var tempBytes = BitConverter.GetBytes(tempVelue);
                writeBytesList.AddRange(tempBytes);
            }
            else if (paramValue is Int16)
            {
                Int16 tempVelue = (Int16)Convert.ChangeType(paramValue, typeof(Int16));
                var tempBytes = BitConverter.GetBytes(tempVelue);
                writeBytesList.AddRange(tempBytes);
            }
            else if (paramValue is Int32)
            {
                Int32 tempVelue = (Int32)Convert.ChangeType(paramValue, typeof(Int32));
                var tempBytes = BitConverter.GetBytes(tempVelue);
                writeBytesList.AddRange(tempBytes);
            }
            else if (paramValue is Int64)
            {
                Int64 tempVelue = (Int64)Convert.ChangeType(paramValue, typeof(Int64));
                var tempBytes = BitConverter.GetBytes(tempVelue);
                writeBytesList.AddRange(tempBytes);
            }
            else if (paramValue is Single)
            {
                Single tempVelue = (Single)Convert.ChangeType(paramValue, typeof(Single));
                var tempBytes = BitConverter.GetBytes(tempVelue);
                writeBytesList.AddRange(tempBytes);
            }
            else if (paramValue is UInt16)
            {
                UInt16 tempVelue = (UInt16)Convert.ChangeType(paramValue, typeof(UInt16));
                var tempBytes = BitConverter.GetBytes(tempVelue);
                writeBytesList.AddRange(tempBytes);
            }
            else if (paramValue is UInt32)
            {
                UInt32 tempVelue = (UInt32)Convert.ChangeType(paramValue, typeof(UInt32));
                var tempBytes = BitConverter.GetBytes(tempVelue);
                writeBytesList.AddRange(tempBytes);
            }
            else if (paramValue is UInt64)
            {
                UInt64 tempVelue = (UInt64)Convert.ChangeType(paramValue, typeof(UInt64));
                var tempBytes = BitConverter.GetBytes(tempVelue);
                writeBytesList.AddRange(tempBytes);
            }
            else if (paramValue is string)
            {
                string tempVelue = (string)Convert.ChangeType(paramValue, typeof(string));
                var tempBytes = System.Text.Encoding.UTF8.GetBytes(tempVelue);
                writeBytesList.AddRange(tempBytes);
            }
            else
            {
                throw new Exception("Can not find type" + typeof(T));
            }
        }



        string
        public T Read<T>(int paramLength)
        {
        }
    }
}


namespace HiSocket.Tcp
{
    /// <summary>
    /// 消息由以下几部分组成:消息长度(ushort)+消息协议(ushort)+具体消息内容
    /// 消息长度是整个套接字的长度(也包含前两位消息长度的占用字节长度)
    /// </summary>
    public interface IMsg
    {
        void Write<T>(T paramValue);


        T Read<T>(int paramLength);
    }
}
