# HiSocket_unity
----------------------


### 如何使用
 可以从此链接下载最新的unity package: [![Github Releases](https://img.shields.io/github/downloads/atom/atom/total.svg)](https://github.com/hiramtan/HiSocket_unity/releases)

 或者从unity asset store下载: [https://www.assetstore.unity3d.com/en/#!/content/104658](https://www.assetstore.unity3d.com/en/#!/content/104658) 

---------

### 功能
- Tcp socket
- Udp socket
- 消息注册和回调
- 二进制字节消息封装
- Protobuf消息封装
- 支持AES消息加密


### 详情
1. Tcp和Udp都是采用异步通信的方式

2. 用户发送接收的操作在主线程中.

3. 实际上后台会有发送线程和接收线程处理数据传输.

4. 监听连接事件获得当前的连接状态.

5. 监听接收事件获得接收的数据.

6. 如果使用Tcp协议需要实现IPackage接口处理粘包拆包.

7. Ping接口因为mono底层的bug会在.net2.0平台报错(.net 4.6 没有问题,或者也可以使用unity的接口获得Ping)

---------


#### Tcp Example
[Transmission Control Protocol](https://en.wikipedia.org/wiki/Transmission_Control_Protocol)

Tcp 协议提供可靠有序的流字节传输,用户需要自己分割数据,在这个框架中可以继承IPackage接口来实现.

[![](https://i1.wp.com/hiramtan.files.wordpress.com/2017/05/11112.png)](https://i1.wp.com/hiramtan.files.wordpress.com/2017/05/11112.png)

``` csharp
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
    void OnReceive(byte[] bytes)
    {
        Debug.Log("receive bytes: " + BitConverter.ToInt32(bytes, 0));
    }
    public class Packer : IPackage
    {
         public void Unpack(IByteArray reader, Queue<byte[]> receiveQueue)
        {
            //get head length or id
        }
        public void Pack(Queue<byte[]> sendQueue, IByteArray writer)
        {
            //add head length or id
        }
    }
```
---------------------

#### Udp Example
[User Datagram Protocol](https://www.assetstore.unity3d.com/en/#!/content/104658) 

Udp协议提供不可靠的报文消息,用户无法知道当前连接状态,但是消息包时完整的.

``` csharp
    private UdpConnection _udp;
    // Use this for initialization
    void Start()
    {
        _udp = new UdpConnection();
        _udp.ReceiveEvent += OnReceive;
        Connect();
        Send();
    }
    void Connect()
    {
        _udp.Connect("127.0.0.1", 7777);
    }
    // Update is called once per frame
    void Update()
    {
        _udp.Run();
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
    void OnReceive(byte[] bytes)
    {
        Debug.Log("receive bytes: " + BitConverter.ToInt32(bytes, 0));
    }
```
-----------------
#### Message Registration Example
``` csharp
    void RegistMsg()
    {
        MsgRegister.Regist("10001", OnMsg_Bytes);
        MsgRegister.Regist("10002", OnMsg_Protobuf);
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
```
点击链接加入QQ群【83596104】：https://jq.qq.com/?_wv=1027&k=5l6rZEr

support: hiramtan@live.com

-------------
MIT License

Copyright (c) [2017] [Hiram]

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.



