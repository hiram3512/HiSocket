using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HiSocket
{
    internal class Assert
    {
        /// <summary>
        /// Make sure the object is not null
        /// </summary>
        /// <param name="obj">object</param>
        /// <param name="info">information</param>
        public static void IsNotNull(object obj, string info)
        {
            if (obj == null)
            {
                throw new NullReferenceException("Null exception: " + info);
            }
        }

    }
}
