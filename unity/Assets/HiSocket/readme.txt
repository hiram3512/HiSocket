you can download newest version from here:https://github.com/hiramtan/HiSocket_unity

There are two kinds of socket in the project: tcp and udp.

1. TestTcp example show how to use tcp connect to server, send and receive data.
2. TestTcp2 example show how to regist message and callback
3. TestUdp example show how to use udp connect to server, send and receiv data.


More Details

1.Tcp and Udp are use async connection

2.There is a send thread to send message

3.There is a receive thread to receive message

4.You use API send and receive message is on main thread

5.You can get current connect state by adding listener of state event.

6.You can receive message by adding listener of receive event.

7.If you use Tcp socket, you should use IPackage interface to pack unpack message.

8.There is ping logic, but because of the bug of mono, it will throw an error on .net2.0(.net 4.6 will be fine, also you can use unity's api to get ping time)
