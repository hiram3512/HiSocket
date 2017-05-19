//*********************************************************************
// Description:
// 1.添加ipv6支持(在支持ipv6的系统/网络适配器中使用ipv6,否则使用ipv4)
// 2.添加多线程支持: 收发各自开启一个线程,并保证线程更新unity托管逻辑畅通.
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
        //register bytes msg and used for receive msg
        MsgManager.Instance.RegisterMsg(110, OnMsg110);

        //
        // you can register many callback here
        //

        //protobuf msg don't need regist, because it's default proto id is 2000 and it defined in msgdefine.cs



        //connect(prefer host names)
        ClientTcp socket = new ClientTcp();
        bool tempIsConnect = socket.Connect("www.baidu.com", 111);
        Debug.Log(tempIsConnect);


        // send byte msg
        MsgBytes tempMsg1 = new MsgBytes(110);//110 is proto id
        //write msg's body
        tempMsg1.Write<int>(100);
        tempMsg1.Write("hello");
        //flush msg and send it out
        tempMsg1.Flush();



        //send protobuf msg
        TestProtobufStruct testProtobufStruct = new TestProtobufStruct();
        testProtobufStruct.x = 100;
        testProtobufStruct.y = 200;
        MsgProtobuf tempMsg2 = new MsgProtobuf();
        tempMsg2.Write(testProtobufStruct);
        tempMsg2.Flush();

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMsg110(MsgBase param)
    {
        var test = param as MsgBytes;
        int temp1 = test.Read<int>(); //100
        string temp2 = test.Read<string>(5); //"hello"

        Debug.Log(temp1 + temp2);
    }
}

public class TestProtobufStruct
{
    public int x;
    public int y;
}