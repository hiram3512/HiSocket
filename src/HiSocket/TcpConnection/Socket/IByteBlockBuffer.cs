/***************************************************************
 * Description: 
 *
 * Documents: https://github.com/hiramtan/HiSocket
 * Author: hiramtan@live.com
***************************************************************/

using System.Collections.Generic;

namespace HiSocket
{
    interface IByteBlockBuffer
    {
        /// <summary>
        /// Size of per block
        /// </summary>
        int Size { get; }

        /// <summary>
        /// How many blocks used
        /// </summary>
        int Count { get; }

        /// <summary>
        /// LinkedList contain block
        /// </summary>
        LinkedList<byte[]> LinkedList { get; }

        /// <summary>
        /// Read Index operater
        /// </summary>
        ReadOperator Reader { get; }

        /// <summary>
        /// Write index operator
        /// </summary>
        WriteOperator Writer { get; }

        /// <summary>
        /// Judge reader and write if in same block
        /// </summary>
        /// <returns></returns>
        bool IsReaderAndWriterInSameNode();

        /// <summary>
        /// Read all bytes inspite they are in diffrent blocks
        /// </summary>
        /// <returns></returns>
        byte[] ReadAllBytes();

        /// <summary>
        /// Write all bytes in to buffer(maybe write in diffrent blocks)
        /// </summary>
        /// <param name="bytes"></param>
        void WriteAllBytes(byte[] bytes);

        /// <summary>
        /// Create a  new block
        /// </summary>
        /// <returns></returns>
        byte[] CreateBlock();
    }
}
