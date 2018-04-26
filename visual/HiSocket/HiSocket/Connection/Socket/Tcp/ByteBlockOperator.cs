/***************************************************************
 * Description: 
 *
 * Documents: https://github.com/hiramtan/HiSocket_unity
 * Author: hiramtan@live.com
***************************************************************/

using System.Collections.Generic;

namespace HiSocket
{
    internal abstract class ByteBlockOperator
    {
        protected IByteBlockBuffer ByteBlockBuffer { get; }
        public LinkedListNode<byte[]> Node { get; set; }
        public int Position { get; set; }
        public ByteBlockOperator(IByteBlockBuffer byteBlockBuffer)
        {
            ByteBlockBuffer = byteBlockBuffer;
            Node = ByteBlockBuffer.LinkedList.First;
        }
    }
}
