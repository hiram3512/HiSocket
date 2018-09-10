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

    public class OneMsg : MsgRegistBase
    {
        public override void Regist()
        {
            MsgRegister.Regist("key1", (x) => { });//This will auto regist
        }
    }

    public class TwoMsg : MsgRegistBase
    {
        public override void Regist()
        {
            MsgRegister.Regist("key2", (x) => { });//This will auto regist
        }
    }
}