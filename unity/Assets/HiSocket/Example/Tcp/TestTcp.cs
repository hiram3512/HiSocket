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
    private ISocket _tcp;
    private IPackage _packer = new Packer();
    // Use this for initialization
    void Start()
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
    void Update()
    {
        _tcp.Run();
    }
    void OnState(SocketState state)
    {
        Debug.Log("current state is: " + state);
        if (state == SocketState.Connected)
        {
            Send();
        }
    }
    void Send()
    {
        for (int i = 0; i < 10; i++)
        {
            var bytes = BitConverter.GetBytes(i);
            _tcp.Send(bytes);
            Debug.Log("send message: " + i);
        }
    }
    private void OnApplicationQuit()
    {
        _tcp.DisConnect();
    }
    void OnReceive(byte[] bytes)
    {
        Debug.Log("receive bytes: " + BitConverter.ToInt32(bytes, 0));
    }
    public class Packer : IPackage
    {
        public void Unpack(IByteArray reader, Queue<byte[]> receiveQueue)
        {
            //get head length or id
            while (reader.Length >= 1)
            {
                byte bodyLength = reader.Read(1)[0];

                if (reader.Length >= bodyLength)
                {
                    var body = reader.Read(bodyLength);
                    receiveQueue.Enqueue(body);
                }
            }
        }

        public void Pack(Queue<byte[]> sendQueue, IByteArray writer)
        {
            //add head length or id
            byte[] head = new Byte[1] { 4 };
            writer.Write(head, head.Length);

            var body = sendQueue.Dequeue();
            writer.Write(body, body.Length);
        }
    }
}