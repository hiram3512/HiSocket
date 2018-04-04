# HiSocket_unity
----------------------


### 如何使用
 可以从此链接下载最新的unity package: [![Github Releases](https://img.shields.io/github/downloads/atom/atom/total.svg)](https://github.com/hiramtan/HiSocket_unity/releases)


---------

### 功能
- Tcp socket
- Udp socket
- 可伸缩字节表
- 高性能字节块缓冲区
- 消息注册和回调
- 二进制字节消息封装
- Protobuf消息封装
- AES消息加密


### 详情
- Tcp和Udp都是采用主线程异步连接的方式(避免主线程阻塞).
- 启动发送线程和接收线程处理数据传输(提高性能).
- 供用户调用发送或接受数据的API在主线程中(方便直接操作unity的组件)
- 监听连接事件获得当前的连接状态.
- 监听接收事件获得接收的数据.
- 存在字节数组队列,方便用来测试和数据重发.
- 高性能字节缓冲区避免内存空间重复申请,减少GC.
- 如果使用Tcp协议需要实现IPackage接口处理粘包拆包.
- Ping接口因为mono底层的bug会在.net2.0平台报错(.net 4.6 没有问题,或者也可以使用unity的接口获得Ping,工程中有示例代码)

---------
### 细节
- Tcp 

    - Tcp connection    
    Tcp协议传输字节流,用户需要分割字节流获得正确的数据包,当创建一个tcp协议的socket时,需要传入一个Package对象来封包和解包.
        ```csharp
        private IPackage _packer = new Packer();
        void Test()
        {
         _tcp = new TcpConnection(_packer);
        }

        public class Packer : IPackage
        {
            public void Unpack(IByteArray reader, Queue<byte[]> receiveQueue)
            {
               //add your unpack logic here
           }

           public void Pack(Queue<byte[]> sendQueue, IByteArray writer)
           {
               // add your pack logic here
           }
        }
        ``` 

    - 连接
        ```csharp
        _tcp.Connect("127.0.0.1", 7777);
        ```

    - 断开连接
    当不再运行时需要主动调用接口断开与服务器的连接(比如响应unity的onapplicationquit执行时)
        ```csharp
        void OnApplicationQuit()
        {
            _tcp.DisConnect();
        }
        ```

    - 连接状态变化
    如果想获取当前的连接状态,可以订阅连接状态事件.
        ```csharp
        void Test()
        {
            _tcp.StateChangeEvent += OnState;
        }
        void OnState(SocketState state)
        {
            Debug.Log("current state is: " + state);
            if (state == SocketState.Connected)
            {
                Debug.Log("connect success");
                //can send or receive message
            }
            else if (state == SocketState.DisConnected)
            {
                Debug.Log("connect failed");
            }
            else if (state == SocketState.Connecting)
            {
                Debug.Log("connecting");
            }
        }
        ```

    - 发送消息
        ```csharp
        void Test()
        {
            var bytes = BitConverter.GetBytes(100);
            _tcp.Send(bytes);
        }
        ```

    - 接受消息
    You can regist receiveevent and when message come from server, this event will be fire.
        ```csharp
            void Test()
            {
                _tcp.ReceiveEvent += OnReceive;
            }
            void OnReceive(byte[] bytes)
            {
                Debug.Log("receive msg: " + BitConverter.ToInt32(bytes, 0));
            }
        ```

    - 封包和解包
    最初创建连接时我们定义了一个packer来分割数据包,当发送消息时我们在数据头部插入消息长度/当接收到消息时我们根据头部的消息长度获得数据包的大小.
        ```csharp
        private bool _isGetHead = false;
        private int _bodyLength;
        public void Unpack(IByteArray reader, Queue<byte[]> receiveQueue)
        {
            if (!_isGetHead)
            {
                if (reader.Length >= 2)//2 is example, get msg's head length
                {
                    var bodyLengthBytes = reader.Read(2);
                    _bodyLength = BitConverter.ToUInt16(bodyLengthBytes, 0);
                }
                else
                {
                    if (reader.Length >= _bodyLength)//get body
                    {
                        var bytes = reader.Read(_bodyLength);
                        receiveQueue.Enqueue(bytes);
                        _isGetHead = false;
                    }
                }
            }
        }
        public void Pack(Queue<byte[]> sendQueue, IByteArray writer)
        {
            var bytesWaitToPack = sendQueue.Dequeue();
            UInt16 length = (UInt16)bytesWaitToPack.Length;//get head lenth
            var bytesHead = BitConverter.GetBytes(length);
            writer.Write(bytesHead);//write head
            writer.Write(bytesWaitToPack);//write body
        }
        ```
- Udp
    - Udp connection
    如果创建upd连接,需要指定发送接收缓冲区大小.
        ```csharp
        _udp = new UdpConnection(1024);
        ```
- Ping
    因为mono在.net2.0和2.0 subset的bug,可以使用如下逻辑获取ping值.
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
- 消息注册
- Protobuf
- 字节消息
- 加密

---------

### Tcp Example
[Transmission Control Protocol](https://en.wikipedia.org/wiki/Transmission_Control_Protocol)

Tcp 协议提供可靠有序的流字节传输,用户需要自己分割数据,在这个框架中可以继承IPackage接口来实现.

[![](https://i1.wp.com/hiramtan.files.wordpress.com/2017/05/11112.png)](https://i1.wp.com/hiramtan.files.wordpress.com/2017/05/11112.png)

``` csharp
    private ITcp _tcp;
    private IPackage _packer = new Packer();
    // Use this for initialization
    void Start()
    {
        _tcp = new TcpConnection(_packer);
        _tcp.StateChangeEvent += OnState;
        _tcp.ReceiveEvent += OnReceive;
        Connect();
    }
    void Update()
    {
        _tcp.Run();
    }

    void Connect()
    {
        _tcp.Connect("127.0.0.1", 7777);
    }
    // Update is called once per frame

    void OnState(SocketState state)
    {
        Debug.Log("current state is: " + state);
        if (state == SocketState.Connected)
        {
            Debug.Log("connect success");
            Send();
        }
        else if (state == SocketState.DisConnected)
        {
            Debug.Log("connect failed");
        }
        else if (state == SocketState.Connecting)
        {
            Debug.Log("connecting");
        }
    }
    void OnApplicationQuit()
    {
        _tcp.DisConnect();
    }
    void Send()
    {
        for (int i = 0; i < 10; i++)
        {
            var bytes = BitConverter.GetBytes(i);
            Debug.Log("send message: " + i);
            _tcp.Send(bytes);
        }
    }
    void OnReceive(byte[] bytes)
    {
        Debug.Log("receive msg: " + BitConverter.ToInt32(bytes, 0));
    }
    public class Packer : IPackage
    {
        public void Unpack(IByteArray reader, Queue<byte[]> receiveQueue)
        {
            //add your unpack logic here
            if (reader.Length >= 1024)//1024 is example, it's msg's length
            {
                var bytesWaitToUnpack = reader.Read(1024);
                receiveQueue.Enqueue(bytesWaitToUnpack);
            }
        }

        public void Pack(Queue<byte[]> sendQueue, IByteArray writer)
        {
            var bytesWaitToPack = sendQueue.Dequeue();
            // add your pack logic here
            //

            writer.Write(bytesWaitToPack);
        }
    }
```
---------------------

### Udp Example
[User Datagram Protocol](https://www.assetstore.unity3d.com/en/#!/content/104658) 

Udp协议提供不可靠的报文消息,用户无法知道当前连接状态,但是消息包时完整的.

``` csharp
    private UdpConnection _udp;
    // Use this for initialization
    void Start()
    {
        _udp = new UdpConnection(1024);
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
    private void OnApplicationQuit()
    {
        _udp.DisConnect();
    }
    void OnReceive(byte[] bytes)
    {
        Debug.Log("receive bytes: " + BitConverter.ToInt32(bytes, 0));
    }
```
-----------------
### Message Registration Example
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



