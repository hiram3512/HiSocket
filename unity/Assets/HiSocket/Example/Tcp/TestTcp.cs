//****************************************************************************
// Description: only use tcp socket to send and receive message
// Should achieve pack and unpack logic by yourself
// Author: hiramtan@live.com
//****************************************************************************

using HiSocket;
using System;
using System.Collections.Generic;
using UnityEngine;

public class TestTcp : MonoBehaviour
{
    private ITcp _tcp;
    private IPackage _packer = new Packer();
    // Use this for initialization
    void Start()
    {
        Init();
    }
    void Update()
    {
        _tcp.Run();
    }

    void Init()
    {
        _tcp = new TcpConnection(_packer);
        _tcp.StateChangeEvent += OnState;
        _tcp.ReceiveEvent += OnReceive;
        Connect();
    }

    void Connect()
    {
        _tcp.Connect("127.0.0.1", 7777);
    }
    // Update is called once per frame

    void OnState(SocketState state)
    {
        Debug.Log("current state is: " + state);
        if (state == SocketState.Connected)
        {
            Debug.Log("connect success");
        }
        else if (state == SocketState.DisConnected)
        {
            Debug.Log("connect failed");
        }
        else if (state == SocketState.Connecting)
        {
            Debug.Log("connecting");
        }
    }
    void Send()
    {
        var bytes = BitConverter.GetBytes(100);
        _tcp.Send(bytes);
    }
    void OnReceive(byte[] bytes)
    {
        Debug.Log("receive bytes: " + bytes.Length);
    }
    public class Packer : IPackage
    {
        public void Unpack(IByteArray reader, Queue<byte[]> receiveQueue)
        {
            //add your unpack logic here
            var bytesWaitToUnpack = reader.Read(1);
            //
            //

            receiveQueue.Enqueue(bytesWaitToUnpack);
        }
        public void Pack(Queue<byte[]> sendQueue, IByteArray writer)
        {
            var bytesWaitToPack = sendQueue.Dequeue();
            // add your pack logic here
            //

            writer.Write(bytesWaitToPack);
        }
    }
}