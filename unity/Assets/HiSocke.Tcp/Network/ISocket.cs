//*********************************************************************
// Description:连接，消息收发，关闭
// Author: hiramtan@live.com
//*********************************************************************


namespace HiSocket.TCP
{
    public interface ISocket
    {
        int buffer { set; }

        bool IsConnected { get; }

        bool Connect(string paramAddress, int paramPort);

        void DisConnect();


        long Ping();

        void Send(byte[] param);

    }
}