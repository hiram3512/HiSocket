/***************************************************************
 * Description: 
 *
 * Documents: https://github.com/hiramtan/HiSocket_unity
 * Author: hiramtan@live.com
***************************************************************/

using System;

namespace HiSocket
{
    internal class Assert
    {
        /// <summary>
        /// Make sure the object is not null
        /// </summary>
        /// <param name="obj">object</param>
        /// <param name="info">information</param>
        public static void NotNull(object obj, string info)
        {
            if (obj == null)
            {
                throw new NullReferenceException(info);
            }
        }

        /// <summary>
        /// Make sure string is not null or empty
        /// </summary>
        /// <param name="obj">string</param>
        /// <param name="info">exception info</param>
        public static void NotNullOrEmpty(string obj, string info)
        {
            if (string.IsNullOrEmpty(obj))
            {
                throw new ArgumentNullException(info);
            }
        }
    }
}