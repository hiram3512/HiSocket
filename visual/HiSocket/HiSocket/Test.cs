//****************************************************************************
// Description:
// Author: hiramtan@qq.com
//***************************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HiSocket.Tcp;
using HiSocket.Udp;

namespace HiSocket
{
    class Test
    {
        void Init()
        {
            ISocket s = new TcpClient(null);
            ISocket ss = new UdpClient(null);
        }
    }
}
