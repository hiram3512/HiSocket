/****************************************************************************
 * Description: use tcp socket to send and receive message
 * Should achieve pack and unpack logic by yourself
 * 
 * Documents: https://github.com/hiramtan/HiSocket_unity
 * Author: hiramtan@live.com
 ****************************************************************************/
using HiSocket;
using System;
using System.Net;
using UnityEngine;

public class TcpExample : MonoBehaviour
{
    TcpConnection _tcp;
    // Use this for initialization
    void Start()
    {
        var package = new PackageExample();
        _tcp = new TcpConnection(package);
        _tcp.OnConnected += OnConnected;
        _tcp.OnReceive += OnReceive;
        //_tcp.OnConnecting
        //_tcp.OnError

        Connect();
    }

    void Connect()
    {
        IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
        IPEndPoint iep = new IPEndPoint(ipAddress, 7777);
        _tcp.Connect(iep);
    }

    void OnConnected()
    {
        Debug.Log("success connect to server");
        Send();
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

    void OnReceive(byte[] bytes)
    {
        Debug.Log("receive msg: " + BitConverter.ToInt32(bytes, 0));
    }
    void OnApplicationQuit()
    {
        _tcp.OnConnected -= OnConnected;
        _tcp.OnReceive -= OnReceive;
        _tcp.DisConnect();
    }










}