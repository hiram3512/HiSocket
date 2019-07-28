/***************************************************************
 * Description: pack and unpack message
 *
 * Documents: https://github.com/hiramtan/HiSocket
 * Author: hiramtan@live.com
***************************************************************/

using HiFramework;
using System;

namespace HiSocket.Tcp
{
    public abstract class PackageBase : IPackage
    {
        /// <summary>
        /// Unpack
        /// </summary>
        /// <param name="source"></param>
        /// <param name="onUnpacked"></param>
        public void Unpack(byte[] source, Action<byte[]> onUnpacked)
        {
            using (BlockBuffer<byte> buffer = new BlockBuffer<byte>(source))
            {
                Unpack(buffer, onUnpacked);
            }
        }

        /// <summary>
        /// Pack
        /// </summary>
        /// <param name="source"></param>
        /// <param name="onPacked"></param>
        public void Pack(byte[] source, Action<byte[]> onPacked)
        {
            using (BlockBuffer<byte> buffer = new BlockBuffer<byte>(source))
            {
                Pack(buffer, onPacked);
            }
        }

        /// <summary>
        /// Sub class achive this methond
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="onUnpacked"></param>
        protected abstract void Unpack(BlockBuffer<byte> bytes, Action<byte[]> onUnpacked);

        /// <summary>
        /// Sub class achive this methond
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="onPacked"></param>
        protected abstract void Pack(BlockBuffer<byte> bytes, Action<byte[]> onPacked);
    }
}
