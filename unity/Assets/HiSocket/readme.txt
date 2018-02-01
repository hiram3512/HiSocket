you can download newest version from here: https://github.com/hiramtan/HiSocket_unity

Features

    Support Tcp socket
    Support Udp socket
    Message registration and call back
    Support byte message
    Support protobuf message
    Support AES encryption

Details

1.Tcp and Udp are use async connection

2.There is a send thread to send message

3.There is a receive thread to receive message

4.You use API send and receive message is on main thread

5.You can get current connect state by adding listener of state event.

6.You can receive message by adding listener of receive event.

7.If you use Tcp socket, you should use IPackage interface to pack unpack message.

8.There is ping logic, but because of the bug of mono, it will throw an error on .net2.0(.net 4.6 will be fine, also you can use unity's api to get ping time)
