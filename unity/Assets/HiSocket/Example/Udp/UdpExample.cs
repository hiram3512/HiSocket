//****************************************************************************
// Description:
// Author: hiramtan@live.com
//****************************************************************************

using System;
using System.Net;
using HiSocket;
using UnityEngine;

public class TestUdp : MonoBehaviour
{
    private UdpConnection _udp;
    // Use this for initialization
    void Start()
    {
        _udp = new UdpConnection(1024);
        _udp.OnReceive += OnReceive;
        Connect();
        Send();
    }
    void Connect()
    {
        _udp.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 7777));
    }
    void Send()
    {
        for (int i = 0; i < 10; i++)
        {
            var bytes = BitConverter.GetBytes(i);
            _udp.Send(bytes);
            Debug.Log("send message: " + i);
        }
    }
    private void OnApplicationQuit()
    {
        _udp.DisConnect();
    }
    void OnReceive(byte[] bytes)
    {
        Debug.Log("receive bytes: " + BitConverter.ToInt32(bytes, 0));
    }
}
