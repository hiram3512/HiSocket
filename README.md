# HiSocket

It is a lightweight client socket solution, you can used it in Unity3d or C# project

![Packagist](https://img.shields.io/packagist/l/doctrine/orm.svg)   [![Build Status](https://travis-ci.org/hiramtan/HiSocket.svg?branch=master)](https://travis-ci.org/hiramtan/HiSocket)   [![GitHub release](https://img.shields.io/github/release/hiramtan/HiSocket.svg)](https://github.com/hiramtan/HiSocket/releases)

-----
[中文说明](https://github.com/hiramtan/HiSocket/blob/master/README_zh.md) 

### How to use

- Use source code: [source code](src/HiSocket)
- Download dll from release path: [![Github Releases](https://img.shields.io/github/downloads/atom/atom/total.svg)](https://github.com/hiramtan/HiSocket/releases) 
- Use Nuget: [HiSocket NuGet package](https://www.nuget.org/packages/HiSocket)

 Quick Start:
```csharp
        //tcp example
        private IPackage package = new PackageExample();
        private TcpConnection tcp;
        void Init()
        {
            tcp = new TcpConnection(package);
            tcp.OnConnected += OnConnected;
            tcp.OnReceive += OnReceive;
            //...
            //...
            tcp.Connect("127.0.0.1",999);
        }
        void OnConnected()
        {
            //connect success
            tcp.Send(new byte[10]);//send message
        }

        void OnReceive(byte[] bytes)
        {
            //get message from server
        }
```

More example:
- C# project example:[Example](https://github.com/hiramtan/HiSocket/tree/master/src/HiSocket.Example)
- Unity project example:[Example](https://github.com/hiramtan/HiSocket/tree/master/unity)

-----

### General
This project contains:
- Connection
    - TcpConnection
        - TcpSocket
        - Package
    - UdpConnection
        - UdpSocket
    - Plugin
- Message
    - Message register
    - Aes encryption
    - Byte message
    - Protobuf message


### Features
- Support Tcp socket
- Support Udp socket
- Scalable byte Array
- High-performance byte block buffer
- Message registration and call back
- Support byte message
- Support protobuf message
- AES encryption


### Details
- Tcp and Udp are all use async connection in main thread(avoid thread blocking).
- Using [Circular_buffer](https://en.wikipedia.org/wiki/Circular_buffer) to avoid memory allocation every time, and reduce garbage collection.
- You can get current connect state and message by adding listener of event.
- If you use Tcp socket, you should implement IPackage interface to pack or unpack message.
- If you use Udp socket, you should declaring buffer size.
- Ping: there is a ping plugin you can used, but if you are used in unity3d because of the bug of mono, it will throw an error on .net2.0(.net 4.6 will be fine, also you can use unity's api to get ping time)


### Advanced
- If you are clear about socket, you also can use TcpSocket(UdpSocket) to achieve your logic, anyway the recommend is TcpConnection(UdpConnection).
- You can use API get socket and do extra logic, for example modify socket's out time
- You can use API get send and receive buffer, for example when disconnect, how to handle buffer's data? just clear or resend to server. 
- OnSocketReceive and OnReceive are diffrent, for example OnSocketReceive size is 100 byte, if user do nothing when uppack OnReceive size is 100. but when user do some zip/unzip(encription.etc) OnReceive size is not 100 anymore. 
- You can add many different plugins based on TcpConnection(UdpConnection) to achieve different functions.
- There are a message register base class help user to quick register id and callback(based on reflection)
- The encryption is use AES, if you want to use encryption you can use the API to encrypte your bytes.
- .etc


### Instructions
- Tcp 
[Transmission Control Protocol](https://en.wikipedia.org/wiki/Transmission_Control_Protocol)

Tcp provides reliable, ordered, and error-checked delivery of a stream of bytes. you have to split bytes by yourself, in this framework you can implement IPackage interface to achieve this.

Because Tcp is a a stream of bytes protocol, user should split the bytes to get correct message package. when create a tcp socket channel there must be a package instance to pack and unpack message.

Pack and Unpack message: In the beginning we define a packager to split bytes, when send message we add length in the head of every message and when receive message we use this length to get how long our message is.
        
- Udp
[User Datagram Protocol](https://www.assetstore.unity3d.com/en/#!/content/104658) 

Udp provides checksums for data integrity, and port numbers for addressing different functions at the source and destination of the datagram. that means you don't know current connect state, but package is integrated.

If use Udp connection shold define send and receive's buffer size.

- Ping :
    Because there is a bug with mono on .net 2.0 and subset in unity3d, you can use logic as below.
    ```csharp
    public int PingTime;
    private Ping p;
    private float timeOut = 1;
    private float lastTime;
    void Start()
    {
        StartCoroutine(Ping());
    }
    IEnumerator Ping()
    {
        p = new Ping("127.0.0.1");
        lastTime = Time.realtimeSinceStartup;
        while (!p.isDone && Time.realtimeSinceStartup - lastTime < 1)
        {
            yield return null;
        }
        PingTime = p.time;
        p.DestroyPing();
        yield return new WaitForSeconds(1);
        StartCoroutine(Ping());
    }
    ```
- Message Register
- Protobuf
- Bytes message
- Encription


---------

### Example
There are many example in **HiSocketExample** project or in **HiSocket.unitypackage**, here is some of them:

Package example:
```csharp
/// <summary>
    /// Example: Used to pack or unpack message
    /// You should inheritance IPackage interface and implement your own logic
    /// </summary>
    class PackageExample : IPackage
    {  /// <summary>
       /// Pack your message here(this is only an example)
       /// </summary>
       /// <param name="source"></param>
       /// <param name="unpackedHandler"></param>
        public void Unpack(IByteArray source, Action<byte[]> unpackedHandler)
        {
            // Unpack your message(use int, 4 byte as head)
            while (source.Length >= 4)
            {
                var head = source.Read(4);
                int bodyLength = BitConverter.ToInt32(head, 0);// get body's length
                if (source.Length >= bodyLength)
                {
                    var unpacked = source.Read(bodyLength);// get body
                    unpackedHandler(unpacked);
                }
                else
                {
                    source.Insert(0, head);// rewrite in, used for next time
                }
            }
        }

        /// <summary>
        /// Unpack your message here(this is only an example)
        /// </summary>
        /// <param name="source"></param>
        /// <param name="packedHandler"></param>
        public void Pack(IByteArray source, Action<byte[]> packedHandler)
        {
            // Add head length to your message(use int, 4 byte as head)
            var length = source.Length;
            var head = BitConverter.GetBytes(length);
            source.Insert(0, head);// add head bytes
            var packed = source.Read(source.Length);
            packedHandler(packed);
        }
    }
```

```csharp
private IPackage package = new PackageExample();
        private TcpConnection _tcp;
        static void Main(string[] args)
        {

        }
        void Init()
        {
            tcp = new TcpConnection(package);
            tcp.OnConnected += OnConnected;
            tcp.OnReceive += Receive;
            //_tcp.OnError
            //_tcp.OnDisconnected
        }
        void OnConnected()
        {
            //connect success
            tcp.Send(new byte[10]);//send message
            tcp.DisConnect();//disconnect
        }

        void Receive(byte[] bytes)
        {
            //get message from server
        }
```

```csharp
 void Init()
        {
            var tcp = new TcpConnection(new PackageExample());
            tcp.AddPlugin(new PingPlugin("ping", tcp));
            //tcp.GetPlugin("ping");
        }
```


support: hiramtan@live.com

-------------
MIT License

Copyright (c) [2017] [Hiram]

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.



