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

public class TcpExample_Unity : MonoBehaviour
{
    private TcpConnection tcp;
    // Use this for initialization
    void Start()
    {
        var ip = IPAddress.Parse("127.0.0.1");
        var iep = new IPEndPoint(ip, 7777);
        tcp = new TcpConnection(new PackageExample());
        tcp.OnConnecting += OnConnecting;
        tcp.OnConnected += OnConnected;
        tcp.OnReceive += OnReceive;
        //tcp.OnDisconnected

        tcp.Connect(iep);//start connect
    }

    void OnConnecting()
    {
        Debug.Log("connecting...");
    }

    void OnConnected()
    {
        Debug.Log("connect success");
        tcp.Send(new byte[10]);
    }

    void OnReceive(byte[] bytes)
    {
        Debug.Log("receive message: " + bytes.Length);
    }

    void OnApplicationQuit()
    {
        tcp.Dispose();
    }
}
