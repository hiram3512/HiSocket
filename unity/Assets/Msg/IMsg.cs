//*********************************************************************
// Description:
// Author: hiramtan@live.com
//*********************************************************************
namespace HiSocket
{
    /// <summary>
    /// 消息由以下几部分组成:消息长度(uint)+消息协议(ushort)+消息内容
    /// 消息长度是整个套接字的长度(也包含前两位消息长度的占用字节长度)
    /// </summary>
    public interface IMsg
    {
        void Write<T>(T paramValue);

        /// <summary>
        /// 读取消息体内容(长度+协议后的消息体内容)
        /// 读取sting类型需要传入长度
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="paramLength">读取string类型长度</param>
        /// <returns></returns>
        T Read<T>(int paramLength = 0);

        void Flush();
    }
}
