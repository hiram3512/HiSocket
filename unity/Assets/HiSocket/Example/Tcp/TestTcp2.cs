//****************************************************************************
// Description:tcp socket and message regist
// Author: hiramtan@live.com
//****************************************************************************

using System;
using UnityEngine;
using HiSocket;
using System.Collections.Generic;

public class TestTcp2 : MonoBehaviour
{
    private ITcp _tcp;
    private IPackage _packer = new Packer();
    // Use this for initialization
    void Start()
    {
        RegistMsg();
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
            Debug.Log("send message: " + i);
            _tcp.Send(bytes);
            i++;
        }
    }
    private void OnApplicationQuit()
    {
        _tcp.DisConnect();
    }
    void OnReceive(byte[] bytes)
    {
        //Debug.Log("receive bytes: " + BitConverter.ToInt32(bytes, 0));
        var byteArray = new ByteArray();
        byteArray.Write(bytes, bytes.Length);
        msgRegister.Dispatch("10001", byteArray);
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
                    receiveQueue.Enqueue(reader.Read(bodyLength));
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


    #region receive message
    IMsgRegister msgRegister = new MsgRegister();
    void RegistMsg()
    {
        msgRegister.Regist("10001", OnMsg_Bytes);
        msgRegister.Regist("10002", OnMsg_Protobuf);
    }

    void OnMsg_Bytes(IByteArray byteArray)
    {
        var msg = new MsgBytes(byteArray);
        int getInt = msg.Read<int>();
    }

    void OnMsg_Protobuf(IByteArray byteArray)
    {
        var msg = new MsgProtobuf(byteArray);
        GameObject testClass = msg.Read<GameObject>();//your class's type
        var testName = testClass.name;
    }
    #endregion
    #region send message
    void Msg_Bytes()
    {
        var msg = new MsgBytes();
        int x = 10;
        msg.Write(x);
        byte[] bytes = msg.ByteArray.Read(msg.ByteArray.Length);
        _tcp.Send(bytes);
    }
    void Msg_Protobuf()
    {
        var msg = new MsgProtobuf();
        var testGo = new GameObject();
        msg.Write(testGo);
        byte[] bytes = msg.ByteArray.Read(msg.ByteArray.Length);
        _tcp.Send(bytes);
    }
    #endregion
}