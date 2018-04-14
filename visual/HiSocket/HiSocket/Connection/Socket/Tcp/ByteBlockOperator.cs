/***************************************************************
 * Description: 
 *
 * Documents: https://github.com/hiramtan/HiSocket_unity
 * Author: hiramtan@live.com
***************************************************************/

using System.Collections.Generic;

namespace HiSocket
{
    public abstract class ByteBlockOperator
    {
        protected ByteBlockBuffer ByteBlockBuffer { get; }
        public LinkedListNode<byte[]> Node { get; set; }
        public int Position { get; set; }
        public ByteBlockOperator(ByteBlockBuffer byteBlockBuffer)
        {
            ByteBlockBuffer = byteBlockBuffer;
            Node = ByteBlockBuffer.LinkedList.First;
        }
    }
}
