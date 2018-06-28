/***************************************************************
 * Description:
 *
 * Author: hiramtan@live.com
***************************************************************/

using System;

namespace HiSocket
{
    public static class Assert
    {

        /// <summary>
        /// Make sure args is true
        /// </summary>
        /// <param name="args"></param>
        public static void IsTrue(bool args)
        {
            if (!args)
            {
                throw new Exception("args is false[{0}]");
            }
        }

        /// <summary>
        /// Make sure args is false
        /// </summary>
        /// <param name="args"></param>
        public static void IsFalse(bool args)
        {
            if (args)
            {
                throw new Exception("args is true[{0}]");
            }
        }

        /// <summary>
        /// Make sure they are equal
        /// </summary>
        /// <param name="args1"></param>
        /// <param name="args2"></param>
        public static void AreEqual(object args1, object args2)
        {
            if (args1 != args2)
            {
                throw new Exception("they are not equal");
            }
        }

        /// <summary>
        /// Make sure they are not equal
        /// </summary>
        /// <param name="args1"></param>
        /// <param name="args2"></param>
        public static void AreNotEqual(object args1, object args2)
        {
            if (args1 == args2)
            {
                throw new Exception("they are equal");
            }
        }

        /// <summary>
        /// Make sure not null
        /// </summary>
        /// <param name="args"></param>
        public static void IsNotNull(object args)
        {
            if (args == null)
            {
                throw new ArgumentNullException("args is null");
            }
        }

        /// <summary>
        /// Make sure is not null or empty
        /// </summary>
        /// <param name="args"></param>
        public static void IsNotNullOrEmpty(string args)
        {
            if (string.IsNullOrEmpty(args))
            {
                throw new Exception("string is null or empty");
            }
        }

        /// <summary>
        /// Make sure is null
        /// </summary>
        /// <param name="args"></param>
        public static void IsNull(object args)
        {
            if (args != null)
            {
                throw new Exception("args is not null");
            }
        }

        /// <summary>
        /// Make sure is null or empty
        /// </summary>
        /// <param name="args"></param>
        public static void IsNullOrEmpty(string args)
        {
            if (!string.IsNullOrEmpty(args))
            {
                throw new Exception("args is not null or empty");
            }
        }

        /// <summary>
        /// Failed
        /// </summary>
        /// <param name="args"></param>
        public static void Fail(string args)
        {
            throw new Exception(args);
        }
    }
}