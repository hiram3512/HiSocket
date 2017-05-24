//*********************************************************************
// Description:Used for registe msg, once msg received from server will response the method you registed before.
// Author: hiramtan@live.com
//*********************************************************************

//#define ByteMsg
#define ProtobufMsg

using System;
using System.Collections.Generic;
using System.Text;

namespace HiSocket
{
    public class MsgManager : Singleton<MsgManager>
    {
        public delegate void msgEventHandler(MsgBase paramMsg);
        public Dictionary<int, msgEventHandler> commonDic = new Dictionary<int, msgEventHandler>();
        public Dictionary<string, msgEventHandler> protobufDic = new Dictionary<string, msgEventHandler>();
        private MsgHandler msgHandler;
        public void Init(MsgHandler param)
        {
            msgHandler = param;
        }
        public void RegisterMsg(string paramKey, msgEventHandler paramMsgHandler)
        {
            if (protobufDic.ContainsKey(paramKey))
                throw new Exception("you have already registe this key: " + paramKey);
            protobufDic.Add(paramKey, paramMsgHandler);
        }
        public void RegisterMsg(int paramKey, msgEventHandler paramMsgHandler)
        {
            if (commonDic.ContainsKey(paramKey))
                throw new Exception("you have already registe this key: " + paramKey);
            commonDic.Add(paramKey, paramMsgHandler);
        }
        public void SendMsg(byte[] param)
        {
            msgHandler.Send(param);
        }

        public void ReceiveMsg(byte[] param)
        {
            param = UnZip(param);
            int tempLength = sizeof(UInt16);//接口IMsg中Length占用字节长度
            byte[] tempBytesNoLength = new byte[param.Length - tempLength];
            Array.Copy(param, tempLength, tempBytesNoLength, 0, tempBytesNoLength.Length);
#if ByteMsg
            ParseByte(tempBytesNoLength);
#elif ProtobufMsg
            ParseProtobuf(tempBytesNoLength);
#endif
        }

        private void ParseByte(byte[] param)
        {
            int tempProtocal = BitConverter.ToUInt16(param, 0);
            if (commonDic.ContainsKey(tempProtocal))
            {
                int tempLength = sizeof(UInt16);//接口IByteMsg中协议占用字节长度
                byte[] tempBytesOnlyBody = new byte[param.Length - tempLength];
                Array.Copy(param, tempLength, tempBytesOnlyBody, 0, tempBytesOnlyBody.Length);

                MsgByte msg = new MsgByte(tempBytesOnlyBody);
                commonDic[tempProtocal](msg);
            }
            else
            {
                throw new Exception("Haven't registe this this message: " + tempProtocal);
            }
        }

        private void ParseProtobuf(byte[] param)
        {
            int tempNameLength = BitConverter.ToUInt16(param, 0);//接口IProtobufMsg中名字占用字节长度
            int tempLength1 = sizeof(UInt16);//接口IProtobufMsg中名字占用字节长度
            string tempName = Encoding.UTF8.GetString(param, tempLength1, tempNameLength);//名字长度
            int tempLength2 = sizeof(UInt16) + tempNameLength;//接口IProtobufMsg中名字占用字节长度+接口IProtobufMsg中名字占用字节长度

            //string tempObjectName = "HiSocket." + tempName;
            //MsgProtobuf tempMsg = System.Activator.CreateInstance(System.Type.GetType(tempObjectName)) as MsgProtobuf;
            //if (tempMsg == null)
            //    throw new Exception("Create object failed");


            if (protobufDic.ContainsKey(tempName))
            {
                byte[] tempBytesOnlyBody = new byte[param.Length - tempLength2];
                Array.Copy(param, tempLength2, tempBytesOnlyBody, 0, tempBytesOnlyBody.Length);

                MsgProtobuf msg = new MsgProtobuf(param);
                protobufDic[tempName](msg);
            }
            else
            {
                throw new Exception("Haven't registe this this message: " + tempName);
            }
        }

        private byte[] UnZip(byte[] param)
        {
            //添加反解压逻辑
            //


            //
            return param;
        }
    }
}