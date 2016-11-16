//*********************************************************************
// Description:
// Author: hiramtan@live.com
//*********************************************************************
using System;
using System.Collections.Generic;

namespace HiSocket.Tcp
{
    public class Msg : IMsg
    {
        private int readIndex;
        /// <summary>
        /// 整个套接字内容(包含长度+协议id)
        /// </summary>
        private byte[] bytesForReadArray;

        /// <summary>
        /// 整个套接字内容(长度+协议+消息体)
        /// 1.长度执行flush时自动处理,
        /// 2.需要调用write接口写入协议
        /// 3.需要调用write接口写入消息体
        /// </summary>
        private List<byte> bytesForWriteList = new List<byte>();

        public Msg()
        {
            bytesForWriteList = new List<byte>();
        }

        #region Read
        public Msg(byte[] paramBytes)
        {
            readIndex = sizeof(uint) + sizeof(ushort);
            bytesForReadArray = paramBytes;
        }
        public T Read<T>(int paramLength = 0)
        {
            if (readIndex >= bytesForReadArray.Length)
            {
                throw new Exception("Index(" + readIndex + ") alread large than list's count(" + bytesForReadArray.Length + ")");
            }

            if (typeof(T) == typeof(byte))
            {
                //object tempValue = Convert.ChangeType(bytesForReadList[readIndex], typeof(T));
                //readIndex += sizeof(byte);
                //return (T)tempValue;

                readIndex += sizeof(byte);
                return (T)Convert.ChangeType(bytesForReadArray[readIndex - sizeof(byte)], typeof(T));
            }
            if (typeof(T) == typeof(bool))
            {
                readIndex += sizeof(bool);
                return (T)Convert.ChangeType(BitConverter.ToBoolean(bytesForReadArray, readIndex - sizeof(bool)), typeof(T));
            }
            if (typeof(T) == typeof(char))
            {
                readIndex += sizeof(char);
                return (T)Convert.ChangeType(BitConverter.ToChar(bytesForReadArray, readIndex - sizeof(char)), typeof(T));
            }
            if (typeof(T) == typeof(double))
            {
                readIndex += sizeof(double);
                return (T)Convert.ChangeType(BitConverter.ToDouble(bytesForReadArray, readIndex - sizeof(double)), typeof(T));
            }
            if (typeof(T) == typeof(short))
            {
                readIndex += sizeof(short);
                return (T)Convert.ChangeType(BitConverter.ToInt16(bytesForReadArray, readIndex - sizeof(short)), typeof(T));
            }
            if (typeof(T) == typeof(int))
            {
                readIndex += sizeof(int);
                return (T)Convert.ChangeType(BitConverter.ToInt32(bytesForReadArray, readIndex - sizeof(int)), typeof(T));
            }
            if (typeof(T) == typeof(long))
            {
                readIndex += sizeof(long);
                return (T)Convert.ChangeType(BitConverter.ToInt64(bytesForReadArray, readIndex - sizeof(long)), typeof(T));
            }
            if (typeof(T) == typeof(float))
            {
                readIndex += sizeof(float);
                return (T)Convert.ChangeType(BitConverter.ToSingle(bytesForReadArray, readIndex - sizeof(float)), typeof(T));
            }
            if (typeof(T) == typeof(ushort))
            {
                readIndex += sizeof(ushort);
                return (T)Convert.ChangeType(BitConverter.ToUInt16(bytesForReadArray, readIndex - sizeof(ushort)), typeof(T));
            }
            if (typeof(T) == typeof(uint))
            {
                readIndex += sizeof(uint);
                return (T)Convert.ChangeType(BitConverter.ToUInt32(bytesForReadArray, readIndex - sizeof(uint)), typeof(T));
            }
            if (typeof(T) == typeof(ulong))
            {
                readIndex += sizeof(ulong);
                return (T)Convert.ChangeType(BitConverter.ToUInt64(bytesForReadArray, readIndex - sizeof(ulong)), typeof(T));
            }
            if (typeof(T) == typeof(string))
            {
                readIndex += paramLength;
                return (T)Convert.ChangeType(System.Text.Encoding.UTF8.GetString(bytesForReadArray, readIndex - paramLength, paramLength), typeof(T));
            }
            throw new Exception("Can not find type" + typeof(T));
        }
        #endregion

        public void Write<T>(T paramValue)
        {
            if (paramValue is byte)
            {
                byte tempVelue = (byte)Convert.ChangeType(paramValue, typeof(byte));
                bytesForWriteList.Add(tempVelue);
            }
            else if (paramValue is bool)
            {
                bool tempVelue = (bool)Convert.ChangeType(paramValue, typeof(bool));
                var tempBytes = BitConverter.GetBytes(tempVelue);
                bytesForWriteList.AddRange(tempBytes);
            }
            else if (paramValue is char)
            {
                char tempVelue = (char)Convert.ChangeType(paramValue, typeof(char));
                var tempBytes = BitConverter.GetBytes(tempVelue);
                bytesForWriteList.AddRange(tempBytes);
            }
            else if (paramValue is double)
            {
                double tempVelue = (double)Convert.ChangeType(paramValue, typeof(double));
                var tempBytes = BitConverter.GetBytes(tempVelue);
                bytesForWriteList.AddRange(tempBytes);
            }
            else if (paramValue is short)
            {
                short tempVelue = (short)Convert.ChangeType(paramValue, typeof(short));
                var tempBytes = BitConverter.GetBytes(tempVelue);
                bytesForWriteList.AddRange(tempBytes);
            }
            else if (paramValue is int)
            {
                int tempVelue = (int)Convert.ChangeType(paramValue, typeof(int));
                var tempBytes = BitConverter.GetBytes(tempVelue);
                bytesForWriteList.AddRange(tempBytes);
            }
            else if (paramValue is long)
            {
                long tempVelue = (long)Convert.ChangeType(paramValue, typeof(long));
                var tempBytes = BitConverter.GetBytes(tempVelue);
                bytesForWriteList.AddRange(tempBytes);
            }
            else if (paramValue is float)
            {
                float tempVelue = (float)Convert.ChangeType(paramValue, typeof(float));
                var tempBytes = BitConverter.GetBytes(tempVelue);
                bytesForWriteList.AddRange(tempBytes);
            }
            else if (paramValue is ushort)
            {
                ushort tempVelue = (ushort)Convert.ChangeType(paramValue, typeof(ushort));
                var tempBytes = BitConverter.GetBytes(tempVelue);
                bytesForWriteList.AddRange(tempBytes);
            }
            else if (paramValue is uint)
            {
                uint tempVelue = (uint)Convert.ChangeType(paramValue, typeof(uint));
                var tempBytes = BitConverter.GetBytes(tempVelue);
                bytesForWriteList.AddRange(tempBytes);
            }
            else if (paramValue is ulong)
            {
                ulong tempVelue = (ulong)Convert.ChangeType(paramValue, typeof(ulong));
                var tempBytes = BitConverter.GetBytes(tempVelue);
                bytesForWriteList.AddRange(tempBytes);
            }
            else if (paramValue is string)
            {
                string tempVelue = (string)Convert.ChangeType(paramValue, typeof(string));
                var tempBytes = System.Text.Encoding.UTF8.GetBytes(tempVelue);
                bytesForWriteList.AddRange(tempBytes);
            }
            else
            {
                throw new Exception("Can not find type" + typeof(T));
            }
        }

        public void Flush()
        {
            uint tempLength = (uint)bytesForWriteList.Count;
            byte[] tempBytes = BitConverter.GetBytes(tempLength);
            bytesForWriteList.InsertRange(0, tempBytes);
            byte[] tempData = bytesForWriteList.ToArray();
            if (MsgManager.iSocket.IsConnected)
                MsgManager.iSocket.Send(tempData);
            else
                throw new Exception("socket havent connected");
        }
    }
}