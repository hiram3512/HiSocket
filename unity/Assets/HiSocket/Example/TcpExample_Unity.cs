/***************************************************************
 * Description: 
 *
 * Documents: https://github.com/hiramtan/HiSocket
 * Author: hiramtan@live.com
***************************************************************/

using HiSocketExample;
using System.Net;
using HiSocket;
using UnityEngine;

public class UnityTcpExample : MonoBehaviour
{
    private TcpConnection _tcp;
    // Use this for initialization
    void Start()
    {
        var ip = IPAddress.Parse("127.0.0.1");
        var iep = new IPEndPoint(ip, 7777);
        _tcp = new TcpConnection(new PackageExample());
        _tcp.OnConnected += OnConnected;
        _tcp.OnReceive += OnReceive;
        //_tcp.OnError
        //_tcp.OnDisconnected


        _tcp.Connect(iep);
    }

    void OnConnected()
    {
        Debug.Log("connect success");
        _tcp.Send(new byte[10]);
    }

    void OnReceive(byte[] bytes)
    {
        Debug.Log("receive message: " + bytes.Length);
    }

    void OnApplicationQuit()
    {
        _tcp.OnConnected -= OnConnected;
        _tcp.OnReceive -= OnReceive;
        _tcp.DisConnect();
    }
}
