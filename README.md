# HiSocket_unity
----------------------
中文说明

### How to use
 You can download unity package from here:

 or you can download from unity asset store: 

---------

### Features
- Support Tcp socket
- Support Udp socket
- Message registration and call back
- Support byte message
- Support protobuf message
- Support AES encryption


### Details
1.Tcp and Udp are use async connection

2.There is a send thread to send message

3.There is a receive thread to receive message

4.You use API send and receive message is on main thread

5.You can get current connect state by adding listener of state event.

6.You can receive message by adding listener of receive event.

7.If you use Tcp socket, you should use IPackage interface to pack unpack message.

8.There is ping logic, but because of the bug of mono, it will throw an error on .net2.0(.net 4.6 will be fine, also you can use unity's api to get ping time)

---------
#### Example1
``` csharp
//****************************************************************************
// Description: only use tcp socket to send and receive message
// Should achieve pack and unpack logic by yourself
// Author: hiramtan@live.com
//****************************************************************************

using HiSocket;
using System;
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
```
#### Example2
``` csharp
//****************************************************************************
// Description:tcp socket and message regist
// Author: hiramtan@live.com
//****************************************************************************

using System;
using UnityEngine;
using HiSocket;

public class TestTcp2 : MonoBehaviour
{
    private ISocket _tcp;
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
        public void Unpack(IByteArray reader, out byte[] writer)
        {
            //get head length
            writer = reader.Read(reader.Length);
        }
        public void Pack(ref byte[] reader, IByteArray writer)
        {
            //add head length
            writer.Write(reader, reader.Length);
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
```


support: hiramtan@live.com

-------------
MIT License

Copyright (c) [2017] [Hiram]

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.



