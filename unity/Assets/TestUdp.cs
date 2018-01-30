//****************************************************************************
// Description:
// Author: hiramtan@live.com
//****************************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using HiSocket;
using UnityEngine;

public class TestUdp : MonoBehaviour
{
    private UdpConnection udp;
	// Use this for initialization
	void Start () {
	    udp = new UdpConnection();

        udp.StateChangeHandler = OnState;
	    udp.ReceiveHandler = OnReceive;

	    udp.Connect("127.0.0.1", 7777);
    }
	
	// Update is called once per frame
	void Update () {

    }
    private void OnApplicationQuit()
    {
        udp.DisConnect();
    }
    private bool isStartSend;
    void OnState(SocketState state)
    {
        if (state == SocketState.Connected)
        {
            isStartSend = true;
        }
        Debug.LogError(state);
    }
    private int i;
    void StartSend()
    {
        if (i > 10)
            isStartSend = false;

        Debug.Log(i);
        var bytes = BitConverter.GetBytes(i);
        udp.Send(bytes);
        i++;
    }

    void OnReceive(byte[] bytes)
    {
        Debug.LogError(BitConverter.ToInt32(bytes, 0));
    }
}
