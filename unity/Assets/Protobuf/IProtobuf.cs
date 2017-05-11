//*********************************************************************
// Description:
// Author: hiramtan@live.com
//*********************************************************************

namespace HiSocket.Protobuf
{
    internal interface IProtobuf
    {
        /// <summary>
        /// 序列化字节流
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="param"></param>
        /// <returns></returns>
        byte[] Serialize<T>(T param);

        /// <summary>
        /// 反序列化字节流
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="param"></param>
        /// <returns></returns>
        T Deserialize<T>(byte[] param);
    }
}
