# HiSocket_unity
----------------------
[中文说明](https://github.com/hiramtan/HiSocket_unity/blob/master/README_zh.md) 

### How to use
 You can download unity package from here: [![Github Releases](https://img.shields.io/github/downloads/atom/atom/total.svg)](https://github.com/hiramtan/HiSocket_unity/releases)

 or you can download from unity asset store:[https://www.assetstore.unity3d.com/en/#!/content/104658](https://www.assetstore.unity3d.com/en/#!/content/104658) 

---------

### Features
- Support Tcp socket
- Support Udp socket
- Message registration and call back
- Support byte message
- Support protobuf message
- Support AES encryption


### Details
1. Tcp and Udp are use async connection

2. The API supply for user to send and receive message is on main thread.

3. Actually there are send thread and receive thread in background to process byte.

4. You can get current connect state by adding listener of state event.

5. You can receive message by adding listener of receive event.

6. If you use Tcp socket, you should implement IPackage interface to pack or unpack message.

7. There is ping logic, but because of the bug of mono, it will throw an error on .net2.0(.net 4.6 will be fine, also you can use unity's api to get ping time)

---------


#### Tcp Example
[Transmission Control Protocol](https://en.wikipedia.org/wiki/Transmission_Control_Protocol)

Tcp provides reliable, ordered, and error-checked delivery of a stream of bytes. you have to split bytes by yourself, in this framework you can implement IPackage interface to achieve this.



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

Udp provides checksums for data integrity, and port numbers for addressing different functions at the source and destination of the datagram. that means you don't know current connect state, but package is integrated.

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


support: hiramtan@live.com

-------------
MIT License

Copyright (c) [2017] [Hiram]

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.



