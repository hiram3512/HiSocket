/***************************************************************
 * Description: 
 *
 * Documents: https://github.com/hiramtan/HiSocket
 * Author: hiramtan@live.com
***************************************************************/

using System;
using System.Reflection;
using HiFramework;

namespace HiSocket.Message
{
    public abstract class BinaryRegistHelper
    {
        /// <summary>
        /// This can quick regist all message
        /// </summary>
        public static void Init()
        {
            var ass = Assembly.GetExecutingAssembly();
            var types = ass.GetTypes();
            var baseType = typeof(BinaryMsgBase);
            for (int i = 0; i < types.Length; i++)
            {
                if (types[i].IsSubclassOf(baseType))
                {
                    var ins = Activator.CreateInstance(types[i]) as BinaryMsgBase;
                    AssertThat.IsNotNull(ins);
                    ins.Regist();
                }
            }
        }
    }
}