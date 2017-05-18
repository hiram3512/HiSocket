//*********************************************************************
// Description:安全单例
// Author: hiramtan@live.com
//*********************************************************************
namespace HiSocket
{
    public class Singleton<T> where T : new()
    {
        private static T instance;
        private static readonly object locker = new object();

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (locker)
                    {
                        if (instance == null)
                        {
                            instance = new T();
                        }
                    }
                }
                return instance;
            }
        }
    }
}