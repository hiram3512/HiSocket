# HiSocket_unity
----------------------
[中文说明](https://github.com/hiramtan/HiSocket_unity/blob/master/README_zh.md) 

### How to use
 You can download unity package from here: [![Github Releases](https://img.shields.io/github/downloads/atom/atom/total.svg)](https://github.com/hiramtan/HiSocket_unity/releases)

---------

### Features
- Support Tcp socket
- Support Udp socket
- High-performance byte block buffer
- Message registration and call back
- Support byte message
- Support protobuf message
- AES encryption


### Details
- Tcp and Udp are all use async connection in main thread(avoid thread blocking).
- There are send thread and receive thread in background to process bytes(high-performance).
- The API for ueser to send and receive message is in main thread(so that you can operate unity's component).
- You can get current connect state by adding listener of state event.
- You can receive message by adding listener of receive event.
- There is a bytes array queue, can use for debugging or resend message.
- High-performance buffer avoid memory allocation every time, and reduce garbage collection.
- If you use Tcp socket, you should implement IPackage interface to pack or unpack message.
- There is ping logic, but because of the bug of mono, it will throw an error on .net2.0(.net 4.6 will be fine, also you can use unity's api to get ping time, project contain some example logic)

### Functionality
- Tcp 
[Transmission Control Protocol](https://en.wikipedia.org/wiki/Transmission_Control_Protocol)
    - Tcp connection 
    Because Tcp is a a stream of bytes protocol, user should split the bytes to get correct message package. when create a tcp socket channel there must be a package instance to pack and unpack message.
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
    - Connect
        ```csharp
        _tcp.Connect("127.0.0.1", 7777);
        ```

    - Disconnect
    You shold initiative use API to disconnect to server when application quit(for example there is a unity's API onapplicationquit)
        ```csharp
        void OnApplicationQuit()
        {
            _tcp.DisConnect();
        }
        ```

    - Connection's state change
    If you want to know current socket channel's state, you can regist state event.
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

    - Send message
        ```csharp
        void Test()
        {
            var bytes = BitConverter.GetBytes(100);
            _tcp.Send(bytes);
        }
        ```

    - Receive message
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

    - Pack and Unpack message
    In the beginning we define a packager to split bytes, when send message we add length in the head of every message and when receive message we use this length to get how long our message is.
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
    If use Udp connection shold define send and receive's buffer size.
        ```csharp
        _udp = new UdpConnection(1024);
        ```
- Ping


---------


### Tcp Example


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

### Udp Example
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


support: hiramtan@live.com

-------------
MIT License

Copyright (c) [2017] [Hiram]

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.



