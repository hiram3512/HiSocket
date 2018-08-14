/***************************************************************
 * Description: 
 *
 * Documents: https://github.com/hiramtan/HiSocket
 * Author: hiramtan@live.com
***************************************************************/

namespace HiSocket
{
    public interface IByteArray
    {
        /// <summary>
        /// return the length of bytearray
        /// </summary>
        int Length { get; }

        /// <summary>
        /// read bytes from head of the list
        /// </summary>
        /// <param name="index">from index</param>
        /// <param name="length">read how many bytes</param>
        /// <returns></returns>
        byte[] Read(int length);

        /// <summary>
        /// write bytes into the end of the list
        /// </summary>
        /// <param name="insertIndex">where to insert</param>
        /// <param name="bytes">bytes wait to write</param>
        void Write(byte[] bytes);
        /// <summary>
        /// insert bytes to array
        /// </summary>
        /// <param name="index"></param>
        /// <param name="bytes"></param>
        void Insert(int index, byte[] bytes);
    }
}
