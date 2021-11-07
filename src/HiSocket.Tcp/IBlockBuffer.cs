using System;
using System.Collections.Generic;
using System.Text;

namespace HiSocket.Tcp
{
    public interface IBlockBuffer<T>
    {
        T[] Buffer { get; }
        int TotalCapcity { get; }
        int Index { get; }



        /// <summary>
        /// Always read from 0 index, and also move length
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        T[] Read(int count);

        void WriteHead(T[] data, int sourceIndex, int sourceLength);



        /// <summary>
        /// Also write at length index, and also move length
        /// </summary>
        /// <param name="data"></param>
        void WriteEnd(T[] data, int sourceIndex, int sourceLength);


        int GetCurrentCapcity();

        void IncreaseIndex(int length);

        void DecreaseIndex(int length);

        void RemoveFront(int length);
    }
}
