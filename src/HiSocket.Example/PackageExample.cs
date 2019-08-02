using HiFramework;
using HiSocket.Tcp;
using System;

namespace HiSocket.Example
{
    public class PackageExample:PackageBase
    {
        protected override void Pack(BlockBuffer<byte> bytes, Action<byte[]> onPacked)
        {
            //Use int as header
            int length = bytes.WritePosition;
            var header = BitConverter.GetBytes(length);
            var newBytes = new BlockBuffer<byte>(length + header.Length);
            //Write header and body to buffer
            newBytes.Write(header);
            newBytes.Write(bytes.Buffer);
            //Notice pack funished
            onPacked(newBytes.Buffer);
        }

        protected override void Unpack(BlockBuffer<byte> bytes, Action<byte[]> onUnpacked)
        {
            //Because header is int and cost 4 byte
            while (bytes.WritePosition > 4)
            {
                int length = BitConverter.ToInt32(bytes.Buffer, 0);
                //If receive body
                if (bytes.WritePosition >= 4 + length)
                {
                    bytes.MoveReadPostion(4);
                    var data = bytes.Read(length);
                    //Notice unpack finished
                    onUnpacked(data);
                    bytes.ResetIndex();
                }
            }
        }
    }
}
