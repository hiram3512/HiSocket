//*********************************************************************
// Description:
// 1.添加ipv6支持(在支持ipv6的系统/网络适配器中使用ipv6,否则使用ipv4)
// 2.添加多线程支持: 收发各自开启一个线程,并保证线程更新unity托管逻辑畅通.
//   ps.线程更新c#托管逻辑无需特殊处理
// Author: hiramtan@live.com
//*********************************************************************

using System;
using HiSocket;
using HiSocket.Msg;
using HiSocket.Tcp;
using UnityEngine;

public class Example : MonoBehaviour
{
    private readonly IMsgRegister _register = new MsgRegister();

    private ISocket socket;
    // Use this for initialization
    private void Start()
    {
        Debug.LogError("start");
        IPackage iPackage = new Package();
        socket = new TcpClient(iPackage);
        socket.StateEvent += OnStateChange;

        socket.Connect("127.0.0.1", 7777);

        //_register.Regist(100, OnMsg);
        //socket.DisConnect();
    }

    private void OnStateChange(SocketState state)
    {
        Debug.Log(state);
        if (state == SocketState.Connected)
        {
            int i = 0;
            while (i < 100)
            {
                var bytes = BitConverter.GetBytes(i);
                socket.Send(bytes);
                Debug.Log(i);
                i++;
            }
        }
    }

    private void OnMsg(IByteArray bytes) //接收派发消息
    {
        //var msg = new MsgBytes(bytes);
        //var test1 = msg.Read<int>();

        //var msg = new MsgProtobuf(bytes);
        //var test1 = msg.Read<MonoBehaviour>();
    }
}