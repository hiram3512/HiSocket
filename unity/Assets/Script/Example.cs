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

public class Example : MonoBehaviour, IPackage
{
    IMsgRegister register = new MsgRegister();
    // Use this for initialization
    void Start()
    {
        ISocket socket = new TcpClient(this);
        socket.StateEvent += OnStateChange;

        socket.Connect("127.0.0.1", 7777);
        socket.Send(new byte[1]);

        register.Regist(100, OnMsg);

        socket.DisConnect();
    }

    void OnStateChange(SocketState state)
    {
        Debug.Log(state);
    }

    void OnMsg(byte[] bytes)//接收派发消息
    {

    }

    public void Unpack(IByteArray bytes)
    {
        //解包:粘包处理
        throw new System.NotImplementedException();

        if (bytes.Length > 2)
        {
            var t1 = bytes.Read(2);
            var t2 = BitConverter.ToInt16(t1, 0);
            //需要粘包处理
            //isgethead = true

            register.Dispatch(t2, bytes.Read(bytes.Length));//派发消息

        }
    }

    public void Pack(IByteArray bytes)
    {
        //封包
        throw new System.NotImplementedException();

        short id = 100;
        var t1 = BitConverter.GetBytes(id);
        bytes.Insert(0, t1);//插入消息头
    }
}