using System.Threading;

/// <summary>
/// 使用多线程收发消息
/// 为了规避线程不能维护unity组件的问题，把线程收发的消息放入消息队列
/// 在主线程通过update来处理收发的消息（因此消息会等到渲染帧时处理，会造成一定时间的延迟）
/// </summary>

public interface IThread
{
    Thread sendThread { get; }
    Thread receiveThread { get; }
}
