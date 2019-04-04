/***************************************************************
 * Description: 
 *
 * Documents: https://github.com/hiramtan/HiSocket
 * Author: hiramtan@live.com
***************************************************************/

using HiSocket;
using HiSocketExample;
using System;
using System.Net;
using UnityEngine;

public class Example : MonoBehaviour
{
    private TcpConnection _tcp;

    private TestServer _server = new TestServer();
    private bool _isConnected;
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
        Debug.Log("<color=green>connecting...</color>");
    }

    void OnConnected()
    {
        Debug.Log("<color=green>connected</color>");
        _isConnected = true;
    }

    private int _counter;

    void Update()
    {
        if (_isConnected)
        {
            Debug.Log("send:" + _counter);
            var data = BitConverter.GetBytes(_counter);
            _tcp.Send(data);
            _counter++;
            if (_counter > 1000)
                _isConnected = false;
        }
    }


    void OnReceive(byte[] bytes)
    {
        var data = BitConverter.ToInt32(bytes, 0);
        Debug.Log("receive: " + data);
    }

    void OnApplicationQuit()
    {
        _tcp.Dispose();
        _server.Close();
    }
}
