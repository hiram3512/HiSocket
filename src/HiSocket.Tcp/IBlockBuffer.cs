/***************************************************************
 * Description: Block buffer for reuse array
 * 
 * Documents: https://github.com/hiram3512/HiSocket
 * Support: hiramtan@live.com
***************************************************************/

using System;
using System.Collections.Generic;
using System.Text;

namespace HiSocket.Tcp
{
    /// <summary>
    /// Block buffer, mostly write at end, read from head
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBlockBuffer<T> : IDisposable
    {
        /// <summary>
        /// Buffer contain data
        /// </summary>
        T[] Buffer { get; }

        /// <summary>
        /// Capcity of buffer
        /// </summary>
        int TotalCapcity { get; }

        /// <summary>
        /// Current index position, this mean how many data store currently
        /// </summary>
        int Index { get; }

        /// <summary>
        /// Always read from 0 index, and also move length
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        T[] ReadFromHead(int count);

        /// <summary>
        /// Always read from 0 index, and don't move length
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        T[] TryReadFromHead(int count);

        /// <summary>
        /// Write at end and move index
        /// </summary>
        /// <param name="data"></param>
        void WriteAtEnd(T[] data, int sourceIndex, int sourceLength);

        /// <summary>
        /// Write at end and move index
        /// </summary>
        /// <param name="data"></param>
        void WriteAtEnd(T[] data);

        /// <summary>
        /// Current capcity
        /// </summary>
        /// <returns></returns>
        int GetCurrentCapcity();

        /// <summary>
        /// IncreaseIndex
        /// </summary>
        /// <param name="length"></param>
        void IncreaseIndex(int length);
    }
}
