# HiSocket

It is a lightweight client socket solution, you can used it in Unity3d or C# project

![Packagist](https://img.shields.io/packagist/l/doctrine/orm.svg)   [![Build Status](https://travis-ci.org/hiramtan/HiSocket.svg?branch=master)](https://travis-ci.org/hiramtan/HiSocket)   [![GitHub release](https://img.shields.io/github/release/hiramtan/HiSocket.svg)](https://github.com/hiramtan/HiSocket/releases)

-----
[中文说明](https://github.com/hiramtan/HiSocket/blob/master/README_zh.md) 

### How to use

- Use source code: [source code](/src)
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
- C# project example:[Example](src/HiSocket.Example)
- Unity project example:[Example](src/HiSocket.Example_Unity)

-----

### General
This project contains:
- Tcp
    - TcpConnection
    - TcpSocket
    - Plugin
      - Ping
      - Statistical
- Message
    - Binary Message
    - Protobuf Message
    - Aes encryption
- BlockBuffer

### Features
- Support Tcp socket
- Scalable byte Array
- High-performance byte block buffer
- Message registration and call back
- Support byte message
- Support protobuf message
- AES encryption


### Details
- Use async connection in main thread(avoid thread blocking).
- Using [Circular_buffer](https://en.wikipedia.org/wiki/Circular_buffer) to avoid memory allocation every time, and reduce garbage collection.
- You can get current connect state and message by adding listener of event.
- If you use Tcp socket, you should implement IPackage interface to pack or unpack message.
- Ping: there is a ping plugin you can used, but if you are used in unity3d because of the bug of mono, it will throw an error on .net2.0(.net 4.6 will be fine, also you can use unity's api to get ping time)


### Advanced
- If you are clear about socket, you also can use TcpSocket to achieve your logic, anyway the recommend is TcpConnection.
- You can use API get socket and do extra logic, for example modify socket's out time
- You can use API get send and receive buffer, for example when disconnect, how to handle buffer's data? just clear or resend to server. 
- OnSocketReceive and OnReceive are diffrent, for example OnSocketReceive size is 100 byte, if user do nothing when uppack OnReceive size is 100. but when user do some zip/unzip(encription.etc) OnReceive size is not 100 anymore. 
- You can add many different plugins based on TcpConnection to achieve different functions.
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

---------

### Example
There are many example in **HiSocketExample** project or in **HiSocket.unitypackage**, here is some of them:

Package example:
```csharp
public class PackageExample:PackageBase
    {
        protected override void Pack(BlockBuffer<byte> bytes, Action<byte[]> onPacked)
        {
            //Use int as header
            int length = bytes.WritePosition;
            var header = BitConverter.GetBytes(length);
            var newBytes = new BlockBuffer<byte>(length + header.Length);
            //Write header and body to buffer
            newBytes.Write(header);
            newBytes.Write(bytes.Buffer);
            //Notice pack funished
            onPacked(newBytes.Buffer);
        }

        protected override void Unpack(BlockBuffer<byte> bytes, Action<byte[]> onUnpacked)
        {
            //Because header is int and cost 4 byte
            while (bytes.WritePosition > 4)
            {
                int length = BitConverter.ToInt32(bytes.Buffer, 0);
                //If receive body
                if (bytes.WritePosition >= 4 + length)
                {
                    bytes.MoveReadPostion(4);
                    var data = bytes.Read(length);
                    //Notice unpack finished
                    onUnpacked(data);
                    bytes.ResetIndex();
                }
            }
        }
    }
```

```csharp
TcpConnection tcp;
        void Connect()
        {
            tcp = new TcpConnection(new PackageExample());
            tcp.OnDisconnected += OnDisconnect;
            tcp.Connect("127.0.0.1", 999);
            tcp.Socket.NoDelay = true;
            tcp.Socket.SendTimeout = 100;
            tcp.Socket.ReceiveTimeout = 200;
            //...


            // you can add plugin sub from IPlugins
            tcp.AddPlugin(new StatisticalPlugin("Statistical"));//this plugin calculate how many send
        }

        void OnDisconnect()
        {
            var length = tcp.SendBuffer.WritePosition;
            Console.WriteLine("Still have {0} not send to server when abnormal shutdown");
            var data = tcp.SendBuffer.Read(length);
            tcp.SendBuffer.ResetIndex();

            //use can handle these data, for example maybe can send next time when connect again
            //tcp.Send(data);
        }
```

```csharp
 /// <summary>
    /// The recommend is use TcpConnection 
    /// </summary>
    class Example3
    {
        TcpSocket tcp; //The recommend is use TcpConnection 
        void Connect()
        {
            tcp = new TcpSocket(1024);//set buffer size
            tcp.OnReceiveBytes += OnReceive;
            tcp.Connect("127.0.0.1", 999);
        }

        void OnReceive(byte[] bytes)
        {
            //split bytes here
        }
    }
```


support: hiramtan@live.com

-------------
MIT License

Copyright (c) [2017] [Hiram]

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.



