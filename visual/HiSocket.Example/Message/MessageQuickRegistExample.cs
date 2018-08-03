using HiSocket.Message;

namespace HiSocket.Example
{
    class MessageQuickRegistExample
    {
        void Main()
        {
            MsgRegistBase.Init();
        }
    }

    class PerMsg : MsgRegistBase
    {
        public override void Regist()
        {
            MsgRegister.Regist("key", (x) => { });
        }
    }
}