//****************************************************************************
// Description:
// Author: hiramtan@live.com
//****************************************************************************

using HiSocket;
using System;
using UnityEngine;

public class TestTcp : MonoBehaviour
{
    private TcpConnection _tcp;
    private Packer _packer = new Packer();
    // Use this for initialization
    void Start()
    {
        _tcp = new TcpConnection(_packer);
        _tcp.StateChangeEvent += OnState;
        _tcp.ReceiveEvent += OnReceive;

        _tcp.Connect("127.0.0.1", 7777);
    }

    // Update is called once per frame
    void Update()
    {
        _tcp.Run();
    }

    private void OnApplicationQuit()
    {
        _tcp.ReceiveEvent -= OnReceive;
        _tcp.DisConnect();
    }

    private bool isStartSend;
    void OnState(SocketState state)
    {
        Debug.Log("current state is: " + state);
        if (state == SocketState.Connected)
        {
        }
    }

    private int i;
    void StartSend()
    {
        if (i > 10)
            isStartSend = false;

        Debug.Log(i);
        var bytes = BitConverter.GetBytes(i);
        _tcp.Send(bytes);
        i++;
    }

    void OnReceive(byte[] bytes)
    {
        Debug.Log("receive bytes: " + BitConverter.ToInt32(bytes, 0));
    }

    public class Packer : IPackage
    {
        public void Unpack(IByteArray reader, out byte[] writer)
        {
            //get head length or id
            writer = reader.Read(reader.Length);
        }

        public void Pack(ref byte[] reader, IByteArray writer)
        {
            //add head length or id
            writer.Write(reader, reader.Length);
        }
    }
}
