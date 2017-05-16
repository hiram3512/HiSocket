//*********************************************************************
// Description:
// Author: hiramtan@live.com
//*********************************************************************

using System;
using System.Collections.Generic;
using System.Text;

namespace HiSocket
{
    public class MsgManager
    {
        public static MsgManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new MsgManager();
                return instance;
            }
        }
        private static MsgManager instance;
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
        //public void SendLuaMsg(string paramName)
        //{
        //    msgHandler.Send(param);
        //}
        public void ReceiveMsg(byte[] paramBytes)
        {
            paramBytes = UnZip(paramBytes);
            int id = (UInt16)BitConverter.ToUInt16(paramBytes, 0);
            if (id == MsgDefine.protobufID)
            {
                int length = MsgDefine.id + MsgDefine.order + MsgDefine.time;
                string name = GetString(paramBytes, length);
                if (!protobufDic.ContainsKey(name))
                    return;
                MsgProtobuf msg = new MsgProtobuf(paramBytes);
                protobufDic[name](msg);
            }
            else
            {
                if (!commonDic.ContainsKey(id))
                    return;
                MsgBytes msg = new MsgBytes(paramBytes);
                commonDic[id](msg);
            }
        }
        private string GetString(byte[] paramBytes, int paramIndex)
        {
            int length = 0;
            for (int i = 0; i < paramBytes.Length - paramIndex; i++)
            {
                if (paramBytes[paramIndex + i + 1] == 0)
                {
                    length = paramIndex + i;
                    break;
                }
            }
            string value = Encoding.UTF8.GetString(paramBytes, paramIndex, length);
            return value;
        }

        private byte[] UnZip(byte[] param)
        {
            //添加反解压逻辑
            return null;
        }
    }
}