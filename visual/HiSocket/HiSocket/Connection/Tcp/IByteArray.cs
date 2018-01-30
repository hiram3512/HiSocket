//****************************************************************************
// Description:
// Author: hiramtan@live.com
//***************************************************************************

namespace HiSocket
{
    public interface IByteArray
    {
        int Length { get; }
        byte[] Read(int length);
        void Write(byte[] bytes, int length);
        /// <summary>
        /// 从index位开始插入数据
        /// </summary>
        /// <param name="index"></param>
        /// <param name="bytes"></param>
        void Insert(int index, byte[] bytes);

        void Clear();
    }
}
