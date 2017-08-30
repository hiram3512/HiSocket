//*********************************************************************
// Description:
// 1.添加ipv6支持(在支持ipv6的系统/网络适配器中使用ipv6,否则使用ipv4)
// 2.添加多线程支持: 收发各自开启一个线程,并保证线程更新unity托管逻辑畅通.
//   ps.线程更新c#托管逻辑无需特殊处理
// Author: hiramtan@live.com
//*********************************************************************


using HiSocket;
using HiSocket.Msg;
using HiSocket.Tcp;
using UnityEngine;

public class Example : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        Package package = new Package();
        ISocket socket = new TcpClient(package);
        socket.StateEvent += OnStateChange;

        socket.Connect("127.0.0.1", 7777);
        socket.Send(new byte[1]);

        IMsgRegister register = new HiSocket.Msg.MsgRegister();
        register.Regist(1,OnMsg);

        socket.DisConnect();
    }

    void OnStateChange(SocketState state)
    {
        Debug.Log(state);
    }

    void OnMsg(byte[] bytes)
    {

    }
}