//*********************************************************************
// Description:
// 1.添加ipv6支持(在支持ipv6的系统/网络适配器中使用ipv6,否则使用ipv4)
// 2.添加多线程支持: 收发各自开启一个线程,并保证线程更新unity托管逻辑畅通.
// 3.默认使用主线程收发（单线程），如果想开启多线程到ClientTcp.cs开启多线程宏定义
// 4.默认使用protobuf数据包收发，如果想采用传统字节流到MsgManager.cs开启宏定义
//   ps.线程更新c#托管逻辑无需特殊处理
// Author: hiramtan@live.com
//*********************************************************************


using HiSocket;
using HiSocket.TCP;
using UnityEngine;

public class Example : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        //registe bytes msg
        MsgManager.Instance.RegisterMsg(110, OnByteMsg);
        //you can registe many msg here
        //....

        //registe protobuf msg
        MsgManager.Instance.RegisterMsg(typeof(TestProtobufStruct).FullName, OnProtobufMsg);
        //....

        //connect(prefer host names)
        ClientTcp socket = new ClientTcp();
        bool tempIsConnect = socket.Connect("www.google.com", 111);
        Debug.Log("是否连接成功： " + tempIsConnect);

        // send byte msg
        MsgByte tempMsg1 = new MsgByte(110);//110 is proto id
        tempMsg1.Write<int>(100);//write msg's body
        tempMsg1.Write("hello");//write msg's body
        tempMsg1.Flush();//send

        //send protobuf msg
        TestProtobufStruct testProtobufStruct = new TestProtobufStruct();
        testProtobufStruct.x = 100;
        testProtobufStruct.y = "hello";
        MsgProtobuf tempMsg2 = new MsgProtobuf();
        tempMsg2.Write(testProtobufStruct);
        tempMsg2.Flush();//send
    }

    void OnByteMsg(MsgBase param)
    {
        var test = param as MsgByte;
        int temp1 = test.Read<int>(); //100
        string temp2 = test.Read<string>(5); //"hello"

        Debug.Log(temp1 + temp2);
    }

    void OnProtobufMsg(MsgBase param)
    {
        var test = param as MsgProtobuf;
        var test2 = test.Read<TestProtobufStruct>();

        int temp1 = test2.x;//100
        string temp2 = test2.y;//"hello"
        Debug.Log(temp1 + temp2);
    }
}
public class TestProtobufStruct
{
    public int x;
    public string y;
}