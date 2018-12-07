/***************************************************************
 * Description: 
 *
 * Documents: https://github.com/hiramtan/HiSocket
 * Author: hiramtan@live.com
***************************************************************/

using System;
using System.Collections;
using HiSocketExample;
using System.Net;
using System.Threading;
using HiSocket;
using UnityEngine;

public class Example : MonoBehaviour
{
    private TcpConnection _tcp;

    private TestServer _testServer = new TestServer();

    private bool _isSendOn;
    // Use this for initialization
    void Start()
    {
        var ip = IPAddress.Parse("127.0.0.1");
        var iep = new IPEndPoint(ip, 7777);
        _tcp = new TcpConnection(new PackageExample());
        _tcp.OnConnecting += OnConnecting;
        _tcp.OnConnected += OnConnected;
        _tcp.OnReceive += OnReceive;

        _tcp.Connect(iep); //start connect
    }

    void OnConnecting()
    {
        Debug.Log("connecting...");
    }

    void OnConnected()
    {
        Debug.Log("connect success");
        _isSendOn = true;
    }

    private int _counter;

    void Update()
    {
        Debug.Log("send:" + _counter);
        var data = BitConverter.GetBytes(_counter);
        _tcp.Send(data);
        _counter++;
        if (_counter > 10000)
            _isSendOn = false;
    }


    void OnReceive(byte[] bytes)
    {
        var data = BitConverter.ToInt32(bytes, 0);
        Debug.Log("receive: " + data);
    }

    void OnApplicationQuit()
    {
        if (_tcp != null) _tcp.Dispose();
        if (_testServer != null) _testServer.Close();
    }
}
