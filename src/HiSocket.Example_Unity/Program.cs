/***************************************************************
 * Description: 
 *
 * Documents: https://github.com/hiramtan/HiSocket
 * Author: hiramtan@live.com
***************************************************************/
using HiFramework;
using HiFramework.Unity;
using HiSocket.Example;
using HiSocket.Tcp;
using UnityEngine;

namespace HiSocket.Example_Unity
{
    class Program
    {
        static void Main(string[] args)
        {
        }
    }
    public class Example : MonoBehaviour
    {
        private ITickComponent tick;
        private IMainThread mainThread;
        // Use this for initialization
        void Start()
        {
            Center.Init(new UnityBinder());
            tick = Center.Get<ITickComponent>();
            Connect();
        }

        // Update is called once per frame
        void Update()
        {
            tick.Tick(Time.deltaTime);
        }

        TcpConnection tcp;
        void Connect()
        {
            var package = new PackageExample();
            tcp = new TcpConnection(package);
            tcp.OnReceiveMessage += OnReceive;
            tcp.Connect("127.0.0.1", 999);
        }
       
        void OnReceive(byte[] message)
        {
            mainThread.RunOnMainThread(OnMainThread,message);
        }

        void OnMainThread(object message)
        {
            var data = message as byte[];
        }
    }
}
