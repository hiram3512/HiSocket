//****************************************************************************
// Description:
// Author: hiramtan@live.com
//****************************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HiSocket;

public class NewBehaviourScript : MonoBehaviour
{
    private TcpConnection tcp;
    PackMsg packMsg = new PackMsg();
    // Use this for initialization
    void Start()
    {
        tcp = new TcpConnection(packMsg);
        tcp.StateChangeHandler = OnState;
        tcp.ReceiveHandler = OnReceive;


        tcp.Connect("127.0.0.1", 5077);
    }

    // Update is called once per frame
    void Update()
    {
        tcp.Run();

        if (isStartSend)
            StartSend();
    }

    private void OnApplicationQuit()
    {
        tcp.DisConnect();
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
       var bytes= BitConverter.GetBytes(i);
        tcp.Send(bytes);
        i++;
    }

    void OnReceive(byte[] bytes)
    {
        Debug.LogError(BitConverter.ToInt32(bytes,0));
    }

    public class PackMsg : IPackage
    {
        public void Unpack(IByteArray reader, out byte[] writer)
        {
            writer = reader.Read(reader.Length);
        }

        public void Pack(ref byte[] reader, IByteArray writer)
        {
           writer.Write(reader,reader.Length);
        }
    }
}
