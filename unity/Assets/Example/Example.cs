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
    private readonly IMsgRegister _register = new MsgRegister();

    // Use this for initialization
    private void Start()
    {
        IPackage iPackage = new Package();
        ISocket socket = new TcpClient(iPackage);
        socket.StateEvent += OnStateChange;

        socket.Connect("127.0.0.1", 7777);
        socket.Send(new byte[1]);

        _register.Regist(100, OnMsg);

        socket.DisConnect();
    }

    private void OnStateChange(SocketState state)
    {
        Debug.Log(state);
    }

    private void OnMsg(IByteArray bytes) //接收派发消息
    {
        //var msg = new MsgBytes(bytes);
        //var test1 = msg.Read<int>();


        //var msg = new MsgProtobuf(bytes);
        //var test1 = msg.Read<MonoBehaviour>();
    }
}