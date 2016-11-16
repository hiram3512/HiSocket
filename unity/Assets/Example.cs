//*********************************************************************
// Description:
// Author: hiramtan@live.com
//*********************************************************************
using UnityEngine;
using System.Collections;
using HiSocket.Tcp;

public class Example : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        //regeste msg and used for receive msg
        MsgManager.Register(110, OnMsg110);

        //connect
        SocketTcp socket = new SocketTcp();
        socket.Connect("192.168.1.1", 111, OnConnect);

        // send msg
        Msg temp = new Msg();
        temp.Write<ushort>(110);//write protocal
        //write msg's body
        temp.Write<int>(100);
        temp.Write("hello");
        temp.Flush();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //连接成功
    void OnConnect()
    {
        Debug.Log("connect server success");
    }


    void OnMsg110(IMsg param)
    {
        int temp1 = param.Read<int>();//100
        string temp2 = param.Read<string>(5);//"hello"
    }

}
