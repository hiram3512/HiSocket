/***************************************************************
 * Description: 
 *
 * Documents: https://github.com/hiramtan/HiSocket_unity
 * Author: hiramtan@live.com
***************************************************************/

using System;
using System.Reflection;

namespace HiSocket.Message
{
    public abstract class MsgRegistBase
    {
        public static void Init()
        {
            var ass = Assembly.GetExecutingAssembly();
            var types = ass.GetTypes();
            var baseType = typeof(MsgRegistBase);
            for (int i = 0; i < types.Length; i++)
            {
                if (types[i].IsSubclassOf(baseType))
                {
                    var ins = Activator.CreateInstance(types[i]) as MsgRegistBase;
                    ins.Regist();
                }
            }
        }
        public abstract void Regist();
    }
}