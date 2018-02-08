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
        /// read bytes from index
        /// </summary>
        /// <param name="index">from index</param>
        /// <param name="length"></param>
        /// <returns></returns>
        byte[] Read(int index, int length);
        /// <summary>
        /// read bytes, default index is 0
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        byte[] Read(int length);

        /// <summary>
        /// write bytes to bytearray
        /// </summary>
        /// <param name="index">where to insert</param>
        /// <param name="bytes"></param>
        void Write(int index, byte[] bytes);
        
        /// <summary>
        /// write into bytearray
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="length">how many write into array</param>
        void Write(byte[] bytes, int length);

        void Clear();
    }
}
