//****************************************************************************
// Description:
// Author: hiramtan@qq.com
//***************************************************************************

namespace HiSocket
{
    public interface IByteArray
    {
        int Length { get; }
        byte[] Read(int length);
        void Write(byte[] bytes, int length);

        void Clear();
    }
}
