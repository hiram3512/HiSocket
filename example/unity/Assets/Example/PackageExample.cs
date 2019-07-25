/***************************************************************
 * Description: 
 *
 * Documents: https://github.com/hiramtan/HiSocket
 * Author: hiramtan@live.com
***************************************************************/

using HiFramework;
using HiSocket.Tcp;
using System;

namespace HiSocketExample
{
    /// <summary>
    /// Example: Used to pack or unpack message
    /// You should inheritance IPackage interface and implement your own logic
    /// </summary>
    public class Package : PackageBase
    {
        protected override void Pack(BlockBuffer<byte> bytes, Action<byte[]> onPacked)
        {
            int length = bytes.WritePosition;
            var header = BitConverter.GetBytes(length);
            var newBytes = new byte[length + header.Length];
            Buffer.BlockCopy(header, 0, newBytes, 0, header.Length);
            Buffer.BlockCopy(bytes.Buffer, 0, newBytes, header.Length, length);
            onPacked(newBytes);
        }

        protected override void Unpack(BlockBuffer<byte> bytes, Action<byte[]> onUnpacked)
        {
            while (bytes.WritePosition > 4)
            {
                int length = BitConverter.ToInt32(bytes.Buffer, 0);
                if (bytes.WritePosition >= 4 + length)
                {
                    bytes.MoveReadPostion(4);
                    var data = bytes.Read(length);
                    onUnpacked(data);
                    bytes.ResetIndex();
                }
            }
        }
    }
}
