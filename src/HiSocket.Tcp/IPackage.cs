/***************************************************************
 * Description: pack and unpack message
 *
 * Documents: https://github.com/hiramtan/HiSocket
 * Author: hiramtan@live.com
***************************************************************/

using System;

namespace HiSocket.Tcp
{
    public interface IPackage
    {
        /// <summary>
        /// Handle data receive from server
        /// </summary>
        /// <param name="bytes"></param>
        void Unpack(byte[] source, Action<byte[]> onUnpacked);

        /// <summary>
        /// handle data will send to server
        /// </summary>
        /// <param name="bytes"></param>
        void Pack(byte[] source, Action<byte[]> onPacked);
    }
}
