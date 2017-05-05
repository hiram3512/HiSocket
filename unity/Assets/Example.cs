//*********************************************************************
// Description:
// 1.添加ipv6支持(在支持ipv6的系统/网络适配器中使用ipv6,否则使用ipv4)
// 2.添加多线程支持: 收发各自开启一个线程,并保证线程更新unity托管逻辑畅通.
//   ps.线程更新c#托管逻辑无需特殊处理
// Author: hiramtan@live.com
//*********************************************************************

using HiSocket.Tcp;
using UnityEngine;

public class Example : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        //register msg and used for receive msg
        MsgManager.Register(110, OnMsg110);
        //
        // you can register many callback here
        //




        //connect(prefer host names)
        SocketTcp socket = new SocketTcp();
        socket.Connect("www.baidu.com", 111, OnConnect);

        // send msg
        Msg temp = new Msg();
        temp.Write<ushort>(110); //write protocal
        //write msg's body
        temp.Write<int>(100);
        temp.Write("hello");
        //flush msg and send it out
        temp.Flush();
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// 连接成功或失败
    /// </summary>
    /// <param name="param">成功or失败</param>
    void OnConnect(bool param)
    {
        if (param)
            Debug.Log("connect server success");
        else
            Debug.Log("connect server failed");
    }


    void OnMsg110(IMsg param)
    {
        int temp1 = param.Read<int>(); //100
        string temp2 = param.Read<string>(5); //"hello"

        Debug.Log(temp1 + temp2);
    }
}