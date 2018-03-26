//****************************************************************************
// Description:
// Author: hiramtan@live.com
//***************************************************************************

using System;
using System.Text;

namespace HiSocket
{
    public class MsgBytes : MsgBase
    {
        public MsgBytes() : base()
        {

        }

        public MsgBytes(IByteArray byteArray) : base(byteArray)
        {
        }
        public T Read<T>(int length = 0)
        {
            if (typeof(T) == typeof(byte))
                return (T)Convert.ChangeType(ByteArray.Read(0,sizeof(byte)), typeof(T));
            if (typeof(T) == typeof(byte[]))
            {
                if (length == 0)
                    throw new Exception("read byte[] length:" + length);
                return (T)Convert.ChangeType(ByteArray.Read(0,length), typeof(T));
            }
            if (typeof(T) == typeof(bool))
                return (T)Convert.ChangeType(ByteArray.Read(0,sizeof(bool)), typeof(T));
            if (typeof(T) == typeof(char))
                return (T)Convert.ChangeType(ByteArray.Read(0, sizeof(char)), typeof(T));
            if (typeof(T) == typeof(double))
                return (T)Convert.ChangeType(ByteArray.Read(0, sizeof(double)), typeof(T));
            if (typeof(T) == typeof(short))
                return (T)Convert.ChangeType(ByteArray.Read(0, sizeof(short)), typeof(T));
            if (typeof(T) == typeof(int))
                return (T)Convert.ChangeType(ByteArray.Read(0, sizeof(int)), typeof(T));
            if (typeof(T) == typeof(long))
                return (T)Convert.ChangeType(ByteArray.Read(0, sizeof(long)), typeof(T));
            if (typeof(T) == typeof(float))
                return (T)Convert.ChangeType(ByteArray.Read(0, sizeof(float)), typeof(T));
            if (typeof(T) == typeof(string))
            {
                //index += length;
                if (length == 0)
                    throw new Exception("read string length:" + length);
                return (T)Convert.ChangeType(Encoding.UTF8.GetString(ByteArray.Read(0, length)), typeof(T));
            }
            if (typeof(T) == typeof(ushort))
                return (T)Convert.ChangeType(ByteArray.Read(0,sizeof(ushort)), typeof(T));
            if (typeof(T) == typeof(uint))
                return (T)Convert.ChangeType(ByteArray.Read(0,sizeof(uint)), typeof(T));
            if (typeof(T) == typeof(ulong))
                return (T)Convert.ChangeType(ByteArray.Read(0, sizeof(ulong)), typeof(T));
            throw new Exception("can not get this type" + typeof(T));
        }

        public void Write<T>(T paramValue)
        {
            if (paramValue is byte)
            {
                var tempVelue = (byte)Convert.ChangeType(paramValue, typeof(byte));
                ByteArray.Write(0, new[] { tempVelue });
            }
            else if (paramValue is bool)
            {
                var tempVelue = (bool)Convert.ChangeType(paramValue, typeof(bool));
                var tempBytes = BitConverter.GetBytes(tempVelue);
                ByteArray.Write(0, tempBytes);
            }
            else if (paramValue is char)
            {
                var tempVelue = (char)Convert.ChangeType(paramValue, typeof(char));
                var tempBytes = BitConverter.GetBytes(tempVelue);
                ByteArray.Write(0, tempBytes);
            }
            else if (paramValue is double)
            {
                var tempVelue = (double)Convert.ChangeType(paramValue, typeof(double));
                var tempBytes = BitConverter.GetBytes(tempVelue);
                ByteArray.Write(0, tempBytes);
            }
            else if (paramValue is short)
            {
                var tempVelue = (short)Convert.ChangeType(paramValue, typeof(short));
                var tempBytes = BitConverter.GetBytes(tempVelue);
                ByteArray.Write(0, tempBytes);
            }
            else if (paramValue is int)
            {
                var tempVelue = (int)Convert.ChangeType(paramValue, typeof(int));
                var tempBytes = BitConverter.GetBytes(tempVelue);
                ByteArray.Write(0, tempBytes);
            }
            else if (paramValue is long)
            {
                var tempVelue = (long)Convert.ChangeType(paramValue, typeof(long));
                var tempBytes = BitConverter.GetBytes(tempVelue);
                ByteArray.Write(0, tempBytes);
            }
            else if (paramValue is float)
            {
                var tempVelue = (float)Convert.ChangeType(paramValue, typeof(float));
                var tempBytes = BitConverter.GetBytes(tempVelue);
                ByteArray.Write(0, tempBytes);
            }
            else if (paramValue is ushort)
            {
                var tempVelue = (ushort)Convert.ChangeType(paramValue, typeof(ushort));
                var tempBytes = BitConverter.GetBytes(tempVelue);
                ByteArray.Write(0, tempBytes);
            }
            else if (paramValue is uint)
            {
                var tempVelue = (uint)Convert.ChangeType(paramValue, typeof(uint));
                var tempBytes = BitConverter.GetBytes(tempVelue);
                ByteArray.Write(0, tempBytes);
            }
            else if (paramValue is ulong)
            {
                var tempVelue = (ulong)Convert.ChangeType(paramValue, typeof(ulong));
                var tempBytes = BitConverter.GetBytes(tempVelue);
                ByteArray.Write(0, tempBytes);
            }
            else if (paramValue is string)
            {
                var tempVelue = (string)Convert.ChangeType(paramValue, typeof(string));
                var tempBytes = Encoding.UTF8.GetBytes(tempVelue);
                ByteArray.Write(0, tempBytes);
            }
            else
            {
                throw new Exception("Can not find type" + typeof(T));
            }
        }
    }
}