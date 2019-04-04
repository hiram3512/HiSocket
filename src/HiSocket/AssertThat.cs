/***************************************************************
 * Description: This is assert logic
 * (ps. why not name as Asset, because when do unit test, this will conflict with unit test's Assert)
 * 
 * Document: https://github.com/hiramtan/HiFramework_unity
 * Author: hiramtan@live.com
 ****************************************************************************/

using System;

namespace HiFramework
{
    public static class AssertThat
    {
        /// <summary>
        /// Make sure args is true
        /// </summary>
        /// <param name="args"></param>
        public static void IsTrue(bool args)
        {
            if (!args)
            {
                throw new Exception("args is false");
            }
        }

        /// <summary>
        /// Make sure args is true with info
        /// </summary>
        /// <param name="args"></param>
        /// <param name="info"></param>
        public static void IsTrue(bool args, string info)
        {
            if (!args)
            {
                throw new Exception("args is false" + FormatInfo(info));
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
                throw new Exception("args is true");
            }
        }

        /// <summary>
        /// Make sure args is false with info
        /// </summary>
        /// <param name="args"></param>
        /// <param name="info"></param>
        public static void IsFalse(bool args, string info)
        {
            if (args)
            {
                throw new Exception("args is true" + FormatInfo(info));
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
        /// Make sure they are equal with info
        /// </summary>
        /// <param name="args1"></param>
        /// <param name="args2"></param>
        /// <param name="info"></param>
        public static void AreEqual(object args1, object args2, string info)
        {
            if (args1 != args2)
            {
                throw new Exception("they are not equal" + FormatInfo(info));
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
        /// Make sure they are not equal with info
        /// </summary>
        /// <param name="args1"></param>
        /// <param name="args2"></param>
        /// <param name="info"></param>
        public static void AreNotEqual(object args1, object args2, string info)
        {
            if (args1 == args2)
            {
                throw new Exception("they are equal" + FormatInfo(info));
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
        /// Make sure not null with info
        /// </summary>
        /// <param name="args"></param>
        /// <param name="info"></param>
        public static void IsNotNull(object args, string info)
        {
            if (args == null)
            {
                throw new ArgumentNullException("args is null" + FormatInfo(info));
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
        /// Make sure is not null or empty with info
        /// </summary>
        /// <param name="args"></param>
        public static void IsNotNullOrEmpty(string args, string info)
        {
            if (string.IsNullOrEmpty(args))
            {
                throw new Exception("string is null or empty" + FormatInfo(info));
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
        /// Make sure is null with info
        /// </summary>
        /// <param name="args"></param>
        /// <param name="info"></param>
        public static void IsNull(object args, string info)
        {
            if (args != null)
            {
                throw new Exception("args is not null" + FormatInfo(info));
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
        /// Make sure is null or empty with info
        /// </summary>
        /// <param name="args"></param>
        /// <param name="info"></param>
        public static void IsNullOrEmpty(string args, string info)
        {
            if (!string.IsNullOrEmpty(args))
            {
                throw new Exception("args is not null or empty" + FormatInfo(info));
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

        /// <summary>
        /// Format info to output
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private static string FormatInfo(string info)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info is null");
            }
            return string.Format("[{0}]", info);
        }
    }
}