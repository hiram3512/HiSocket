/***************************************************************
 * Description: 
 *
 * Documents: https://github.com/hiramtan/HiSocket_unity
 * Author: hiramtan@live.com
***************************************************************/

using System;
using System.Reflection;

namespace HiSocket
{
    public abstract class MsgRegistHelper
    {
        public static void Init()
        {
            var ass = Assembly.GetExecutingAssembly();
            var types = ass.GetTypes();
            var baseType = typeof(MsgRegistHelper);
            for (int i = 0; i < types.Length; i++)
            {
                if (types[i].IsSubclassOf(baseType))
                {
                    var ins = Activator.CreateInstance(types[i]) as MsgRegistHelper;
                    ins.Regist();
                }
            }
        }
        public abstract void Regist();
    }
}