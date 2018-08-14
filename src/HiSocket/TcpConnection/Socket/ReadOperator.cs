/***************************************************************
 * Description:
 *
 * Documents: https://github.com/hiramtan/HiSocket
 * Author: hiramtan@live.com
***************************************************************/

using System;

namespace HiSocket
{
    internal class ReadOperator : ByteBlockOperator
    {
        public ReadOperator(IByteBlockBuffer byteBlockBuffer) : base(byteBlockBuffer)
        {
        }
        /// <summary>
        /// how many bytes you have read out
        /// used to move postion
        /// </summary>
        /// <param name="length"></param>
        public void MovePosition(int length)
        {
            lock (Locker)
            {
                Position += length;
                if (Position > ByteBlockBuffer.Size)
                {
                    throw new Exception("Reader position error");
                }
                if (ByteBlockBuffer.IsReaderAndWriterInSameNode())
                {
                    if (Position > ByteBlockBuffer.Writer.Position)
                    {
                        throw new Exception(
                            "When in same node, Reader's position must not large than Writer's postion");
                    }
                }
                if (Position == ByteBlockBuffer.Size) //current block have already read finish
                {
                    Position = 0;
                    ReaderNodeMove();
                }
            }
        }

        void ReaderNodeMove()
        {
            if (Node.Next == null) //next block is null
            {
                Node = ByteBlockBuffer.LinkedList.First; //read from first block
            }
            else //next block is not null
            {
                Node = Node.Next;
            }
        }

        public int GetHowManyCountCanReadInThisBlock()
        {
            lock (Locker)
            {
                if (Node == ByteBlockBuffer.Writer.Node) // if there are in same block
                    return ByteBlockBuffer.Writer.Position - Position;
                return ByteBlockBuffer.Size - Position; //get rest of this block's bytes
            }
        }
    }
}
