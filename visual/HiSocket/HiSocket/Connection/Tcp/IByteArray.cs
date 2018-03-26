//****************************************************************************
// Description:
// Author: hiramtan@live.com
//***************************************************************************

namespace HiSocket
{
    public interface IByteArray
    {
        /// <summary>
        /// return the length of bytearray
        /// </summary>
        int Length { get; }

        /// <summary>
        /// read bytes
        /// </summary>
        /// <param name="index">from index</param>
        /// <param name="length">read how many bytes</param>
        /// <returns></returns>
        byte[] Read(int index, int length);

        /// <summary>
        /// write bytes
        /// </summary>
        /// <param name="insertIndex">where to insert</param>
        /// <param name="bytes">bytes wait to write</param>
        void Write(int insertIndex, byte[] bytes);
        void Clear();
    }
}
