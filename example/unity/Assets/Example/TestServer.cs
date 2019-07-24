using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class TestServer  {

    private Socket socket;
    private bool isOn = true;

    public TestServer()
    {
        IPEndPoint iep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 7777);
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
        socket.Bind(iep);
        socket.Listen(5);
        socket.NoDelay = true;
        new Thread(Watcher).Start();
    }
    void Watcher()
    {
        byte[] buffer = new byte[1 << 16];
        while (isOn)
        {
            var client = socket.Accept();
            int length = 0;
            while ((length = client.Receive(buffer)) > 0)
            {
                byte[] toSend = new byte[length];
                Array.Copy(buffer, 0, toSend, 0, toSend.Length);
                Console.WriteLine(toSend.Length);
                client.Send(toSend);
            }
        }
    }

    public void Close()
    {
        isOn = false;
        socket.Close();
    }
}
