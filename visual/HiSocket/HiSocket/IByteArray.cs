//****************************************************************************
// Description:
// Author: hiramtan@qq.com
//***************************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
