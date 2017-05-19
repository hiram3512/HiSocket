//*********************************************************************
// Description:
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
                return;
            protobufDic.Add(paramKey, paramMsgHandler);
        }
        public void RegisterMsg(int paramKey, msgEventHandler paramMsgHandler)
        {
            if (commonDic.ContainsKey(paramKey))
                return;
            commonDic.Add(paramKey, paramMsgHandler);
        }
        public void SendMsg(byte[] param)
        {
            msgHandler.Send(param);
        }

        public void ReceiveMsg(byte[] paramBytes)
        {
            paramBytes = UnZip(paramBytes);

#if ByteMsg
            ParseByte(paramBytes);
#elif ProtobufMsg
            ParseProtobuf(paramBytes);
#endif
        }

        private void ParseByte(byte[] param)
        {

            int tempProtocal = BitConverter.ToUInt16(param, 0);
            if (commonDic.ContainsKey(tempProtocal))
            {
                MsgByte msg = new MsgByte(param);
                commonDic[tempProtocal](msg);
            }
            else
            {
                throw new Exception("Haven't registe this this message: " + tempProtocal);
            }
        }

        private void ParseProtobuf(byte[] param)
        {
            int tempNameLength = BitConverter.ToUInt16(param, 0);
            string tempName = Encoding.UTF8.GetString(param, 2, tempNameLength);
            string tempObjectName = "HiSocket." + tempName;
            MsgProtobuf tempMsg = System.Activator.CreateInstance(System.Type.GetType(tempObjectName)) as MsgProtobuf;
            if (tempMsg == null)
                throw new Exception("Create object failed");


            if (protobufDic.ContainsKey(tempName))
            {
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