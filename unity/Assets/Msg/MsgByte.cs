//*********************************************************************
// Description:
// Author: hiramtan@live.com
//*********************************************************************
using System;
using System.Text;

namespace HiSocket
{
    public class MsgByte : MsgBase, IByteMsg
    {
        #region Read
        /// <summary>
        /// 用于解析消息
        /// </summary>
        /// <param name="param"></param>
        public MsgByte(byte[] param) : base(param)
        {
            index += sizeof(UInt16);//接口IByteMsg中协议占用字节长度
        }

        public T Read<T>(int _length = 0)
        {
            T value = default(T);
            if (typeof(T) == typeof(byte))
            {
                index += sizeof(byte);
                return (T)Convert.ChangeType(buffer[index - sizeof(byte)], typeof(T));
            }
            else if (typeof(T) == typeof(byte[]))
            {
                byte[] bs = new byte[_length];
                Array.Copy(buffer, index, bs, 0, _length);
                index += _length;
                return (T)Convert.ChangeType(bs, typeof(T));
            }
            else if (typeof(T) == typeof(bool))
            {
                index += sizeof(bool);
                return (T)Convert.ChangeType(BitConverter.ToBoolean(buffer, index - sizeof(bool)), typeof(T));
            }
            else if (typeof(T) == typeof(char))
            {
                index += sizeof(char);
                return (T)Convert.ChangeType(BitConverter.ToChar(buffer, index - sizeof(char)), typeof(T));
            }
            else if (typeof(T) == typeof(double))
            {
                index += sizeof(double);
                return (T)Convert.ChangeType(BitConverter.ToDouble(buffer, index - sizeof(double)), typeof(T));
            }
            else if (typeof(T) == typeof(short))
            {
                index += sizeof(short);
                return (T)Convert.ChangeType(BitConverter.ToInt16(buffer, index - sizeof(short)), typeof(T));
            }
            else if (typeof(T) == typeof(int))
            {
                index += sizeof(int);
                return (T)Convert.ChangeType(BitConverter.ToInt32(buffer, index - sizeof(int)), typeof(T));
            }
            else if (typeof(T) == typeof(long))
            {
                index += sizeof(long);
                return (T)Convert.ChangeType(BitConverter.ToInt64(buffer, index - sizeof(long)), typeof(T));
            }
            else if (typeof(T) == typeof(float))
            {
                index += sizeof(float);
                return (T)Convert.ChangeType(BitConverter.ToSingle(buffer, index - sizeof(float)), typeof(T));
            }
            else if (typeof(T) == typeof(string))
            {
                index += _length;
                return (T)Convert.ChangeType(Encoding.UTF8.GetString(buffer, index - _length, _length), typeof(T));
            }
            else if (typeof(T) == typeof(ushort))
            {
                index += sizeof(ushort);
                return (T)Convert.ChangeType(BitConverter.ToUInt16(buffer, index - sizeof(ushort)), typeof(T));
            }
            else if (typeof(T) == typeof(uint))
            {
                index += sizeof(uint);
                return (T)Convert.ChangeType(BitConverter.ToUInt32(buffer, index - sizeof(uint)), typeof(T));
            }
            else if (typeof(T) == typeof(ulong))
            {
                index += sizeof(ulong);
                return (T)Convert.ChangeType(BitConverter.ToUInt64(buffer, index - sizeof(ulong)), typeof(T));
            }
            return value;
        }
        #endregion

        #region  Write
        public ushort protocal { get; private set; }
        /// <summary>
        /// 用于创建消息(构造函数写入协议id)
        /// </summary>
        /// <param name="param"></param>
        public MsgByte(UInt16 param) : base()
        {
            protocal = param;
        }
        public void Write<T>(T paramValue)
        {
            if (paramValue is byte)
            {
                byte tempVelue = (byte)Convert.ChangeType(paramValue, typeof(byte));
                list.Add(tempVelue);
            }
            else if (paramValue is bool)
            {
                bool tempVelue = (bool)Convert.ChangeType(paramValue, typeof(bool));
                var tempBytes = BitConverter.GetBytes(tempVelue);
                list.AddRange(tempBytes);
            }
            else if (paramValue is char)
            {
                char tempVelue = (char)Convert.ChangeType(paramValue, typeof(char));
                var tempBytes = BitConverter.GetBytes(tempVelue);
                list.AddRange(tempBytes);
            }
            else if (paramValue is double)
            {
                double tempVelue = (double)Convert.ChangeType(paramValue, typeof(double));
                var tempBytes = BitConverter.GetBytes(tempVelue);
                list.AddRange(tempBytes);
            }
            else if (paramValue is short)
            {
                short tempVelue = (short)Convert.ChangeType(paramValue, typeof(short));
                var tempBytes = BitConverter.GetBytes(tempVelue);
                list.AddRange(tempBytes);
            }
            else if (paramValue is int)
            {
                int tempVelue = (int)Convert.ChangeType(paramValue, typeof(int));
                var tempBytes = BitConverter.GetBytes(tempVelue);
                list.AddRange(tempBytes);
            }
            else if (paramValue is long)
            {
                long tempVelue = (long)Convert.ChangeType(paramValue, typeof(long));
                var tempBytes = BitConverter.GetBytes(tempVelue);
                list.AddRange(tempBytes);
            }
            else if (paramValue is float)
            {
                float tempVelue = (float)Convert.ChangeType(paramValue, typeof(float));
                var tempBytes = BitConverter.GetBytes(tempVelue);
                list.AddRange(tempBytes);
            }
            else if (paramValue is ushort)
            {
                ushort tempVelue = (ushort)Convert.ChangeType(paramValue, typeof(ushort));
                var tempBytes = BitConverter.GetBytes(tempVelue);
                list.AddRange(tempBytes);
            }
            else if (paramValue is uint)
            {
                uint tempVelue = (uint)Convert.ChangeType(paramValue, typeof(uint));
                var tempBytes = BitConverter.GetBytes(tempVelue);
                list.AddRange(tempBytes);
            }
            else if (paramValue is ulong)
            {
                ulong tempVelue = (ulong)Convert.ChangeType(paramValue, typeof(ulong));
                var tempBytes = BitConverter.GetBytes(tempVelue);
                list.AddRange(tempBytes);
            }
            else if (paramValue is string)
            {
                string tempVelue = (string)Convert.ChangeType(paramValue, typeof(string));
                var tempBytes = System.Text.Encoding.UTF8.GetBytes(tempVelue);
                list.AddRange(tempBytes);
            }
            else
            {
                throw new Exception("Can not find type" + typeof(T));
            }
        }
        public void Flush()
        {
            byte[] temp = BitConverter.GetBytes(protocal);
            list.InsertRange(0, temp);
            base.Flush();
        }
        #endregion
    }
}