//****************************************************************************
// Description: socket's api 
// Author: hiramtan@live.com
//****************************************************************************

namespace HiSocket
{
    public interface ITcp : ISocket
    {
        /// <summary>
        /// if connected
        /// </summary>
        bool IsConnected { get; }
    }
}