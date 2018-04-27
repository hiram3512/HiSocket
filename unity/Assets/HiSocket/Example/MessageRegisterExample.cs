//****************************************************************************
// Description:tcp socket and message regist
// Author: hiramtan@live.com
//****************************************************************************

using HiSocket;
using UnityEngine;

public class TestTcp2 : MonoBehaviour
{
    private TcpConnection _tcp;
    // Use this for initialization
    void Start()
    {
        _tcp = new TcpConnection(new PackageExample());
        _tcp.Send(new byte[1]);
        RegistMsg();
    }

    void OnReceive(byte[] bytes)
    {
        var byteArray = new ByteArray();
        byteArray.Write(bytes);
        string id = System.Text.Encoding.UTF8.GetString(byteArray.Read(10));//id = 10001
        MsgRegister.Dispatch(id, byteArray);
    }

    #region receive message
    void RegistMsg()
    {
        MsgRegister.Regist("10001", OnMsg);
    }

    void OnMsg(IByteArray byteArray)
    {
        var msg = new MsgBytes(byteArray);
        int getInt = msg.Read<int>();

        //or:
        //var msg = new MsgProtobuf(byteArray);
        //GameObject testClass = msg.Read<GameObject>();//your class's type
        //var testName = testClass.name;
    }
    #endregion
    #region send message
    void Msg_Bytes()
    {
        var msg = new MsgBytes();
        int x = 10;
        float y = 10.1f;
        msg.Write(x);
        msg.Write(y);
        byte[] bytes = msg.ByteArray.Read(msg.ByteArray.Length);
        _tcp.Send(bytes);
    }
    void Msg_Protobuf()
    {
        var msg = new MsgProtobuf();
        var testGo = new GameObject();
        msg.Write(testGo);
        byte[] bytes = msg.ByteArray.Read(msg.ByteArray.Length);
        _tcp.Send(bytes);
    }
    #endregion
}