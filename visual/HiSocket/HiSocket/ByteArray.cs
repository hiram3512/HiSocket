//****************************************************************************
// Description:
// Author: hiramtan@qq.com
//***************************************************************************

namespace HiSocket
{
    internal class ByteArray : IByteArray
    {


        public void Read()
        {
            throw new System.NotImplementedException();
        }

        public void Write(byte[] bytes)
        {

        }
    }
}

public interface IByteArray
{
    void Read();
    void Write(byte[] bytes);

}
