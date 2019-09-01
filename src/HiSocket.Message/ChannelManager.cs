//****************************************************************************
// Description:
// Author: hiramtan@live.com
//***************************************************************************

using HiFramework;
using HiSocket.Tcp;
using System;
using System.Collections.Generic;

namespace HiSocket.Message
{
    /// <summary>
    /// Manage different tcpConnection in project
    /// </summary>
    public static class ChannelManager
    {
        private static readonly Dictionary<string, ITcpConnection> _connections = new Dictionary<string, ITcpConnection>();

        private static ITcpConnection _tcpConnection;

        /// <summary>
        /// Add new conneciton
        /// </summary>
        /// <param name="name"></param>
        /// <param name="tcpConnection"></param>
        public static void AddConnection(string name, ITcpConnection tcpConnection)
        {
            AssertThat.IsFalse(_connections.ContainsKey(name));
            _connections.Add(name, tcpConnection);
        }

        /// <summary>
        /// Remove a conneciton
        /// </summary>
        /// <param name="name"></param>
        public static void RemoveConnection(string name)
        {
            AssertThat.IsTrue(_connections.ContainsKey(name));
            _connections.Remove(name);
        }

        /// <summary>
        /// Switch to use conneciton
        /// </summary>
        /// <param name="name"></param>
        public static void SwitchChannel(string name)
        {
            AssertThat.IsTrue(_connections.ContainsKey(name));
            _tcpConnection = _connections[name];
        }

        /// <summary>
        /// Send data
        /// </summary>
        /// <param name="bytes"></param>
        public static void Send(byte[] bytes)
        {
            if (_tcpConnection != null)
            {
                _tcpConnection.Send(bytes);
            }
        }

        /// <summary>
        /// Send data
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="index"></param>
        /// <param name="length"></param>
        public static void Send(byte[] bytes, int index, int length)
        {
            if (_tcpConnection != null)
            {
                _tcpConnection.Send(bytes, index, length);
            }
        }

        /// <summary>
        /// Send protobuf obj
        /// </summary>
        /// <param name="obj"></param>
        public static void Send(object obj)
        {
            var fullName = obj.GetType().FullName;
            byte[] bytesName = System.Text.Encoding.UTF8.GetBytes(fullName);
            byte[] bytesNameLength = BitConverter.GetBytes(bytesName.Length);
            byte[] bytesBody = ProtobufSerializer.Serialize(obj);
            var bytes = new Byte[4 + bytesName.Length + bytesBody.Length];
            Buffer.BlockCopy(bytesNameLength,0,bytes,0,4);
            Buffer.BlockCopy(bytesName, 0, bytes, 4, bytesName.Length);
            Buffer.BlockCopy(bytesBody, 0, bytes, 4 + bytesName.Length, bytesBody.Length);
            Send(bytes);
        }
    }
}