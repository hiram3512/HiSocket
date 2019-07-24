/***************************************************************
 * Description: Split byte stream, also you can do encription or zip/unzip here
 *
 * Documents: https://github.com/hiramtan/HiSocket
 * Author: hiramtan@live.com
***************************************************************/

using System;
using System.Collections.Generic;

namespace HiSocket.Example
{
    /// <summary>
    /// Example: Used to pack or unpack message
    /// You should inheritance IPackage interface and implement your own logic
    /// </summary>
    public class PackageExample : IPackage
    {
        private List<byte> _receiveBytes = new List<byte>();
        /// <summary>
        /// Pack your message here(this is only an example)
        /// </summary>
        /// <param name="source"></param>
        /// <param name="unpackedHandler"></param>
        public void Unpack(byte[] source, Action<byte[]> unpackedHandler)
        {
            _receiveBytes.AddRange(source);
            // Unpack your message(use int, 4 byte as head)
            while (_receiveBytes.Count >= 4)
            {
                var headerBytes = new byte[4];
                _receiveBytes.CopyTo(0, headerBytes, 0, 4);
                int bodyLength = BitConverter.ToInt32(headerBytes, 0);// get body's length
                if (source.Length >= bodyLength)
                {
                    _receiveBytes.RemoveRange(0, 4);
                    var data = new byte[bodyLength];
                    _receiveBytes.CopyTo(0, data, 0, bodyLength);
                    unpackedHandler(data);
                    _receiveBytes.RemoveRange(0, bodyLength);
                }
            }
        }

        /// <summary>
        /// Unpack your message here(this is only an example)
        /// </summary>
        /// <param name="source"></param>
        /// <param name="packedHandler"></param>
        public void Pack(byte[] source, Action<byte[]> packedHandler)
        {
            // Add head length to your message(use int, 4 byte as head)
            int length = source.Length;
            var head = BitConverter.GetBytes(length);
            var data = new byte[source.Length + 4];
            Array.Copy(head,0,data,0,4);
            Array.Copy(source, 0, data, 4, source.Length);
            packedHandler(data);
        }
    }
}
