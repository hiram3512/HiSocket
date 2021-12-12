/***************************************************************
 * Description: Block buffer for reuse array
 * 
 * Documents: https://github.com/hiram3512/HiSocket
 * Support: hiramtan@live.com
***************************************************************/

namespace HiSocket.Tcp
{
    /// <summary>
    /// Package interface for spit bytes
    /// </summary>
    public interface IPackage
    {
        /// <summary>
        /// Add head bytes and pack a new bytes to send
        /// </summary>
        /// <param name="message"></param>
        /// <param name="bytes"></param>
        void Pack(byte[] message, IBlockBuffer<byte> sendBuffer);

        /// <summary>
        /// Get data from receive buffer and split by head
        /// </summary>
        /// <param name="receiveBuffer"></param>
        /// <param name="message"></param>
        void Unpack(IBlockBuffer<byte> receiveBuffer, ref byte[] message);
    }
}