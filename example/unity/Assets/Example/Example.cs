/***************************************************************
 * Description: 
 *
 * Documents: https://github.com/hiramtan/HiSocket
 * Author: hiramtan@live.com
***************************************************************/

using HiSocketExample;
using System;
using System.Net;
using HiSocket.Tcp;
using UnityEngine;

public class Example : MonoBehaviour
{
    private TcpConnection _tcp;

    private bool _isConnected;
    // Use this for initialization
    void Start()
    {
        var ip = IPAddress.Parse("127.0.0.1");
        var iep = new IPEndPoint(ip, 7777);
        _tcp = new TcpConnection(new Package());
        _tcp.OnConnecting += OnConnecting;
        _tcp.OnConnected += OnConnected;
        _tcp.OnReceiveMessage += OnReceive;

        _tcp.Connect(iep); //start connect
    }

    void OnConnecting(ITcpSocket s)
    {
        Debug.Log("<color=green>connecting...</color>");
    }

    void OnConnected(ITcpSocket s)
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


    void OnReceive(ITcpConnection c, byte[] bytes)
    {
        var data = BitConverter.ToInt32(bytes, 0);
        Debug.Log("receive: " + data);
    }

    void OnApplicationQuit()
    {
        _tcp.Dispose();
    }
}
