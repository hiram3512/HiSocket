/****************************************************************************
 * Description:
 *
 * Author: hiramtan@live.com
 ****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HiSocket
{
    public class WriteOperator : ByteBlockOperator
    {
        public WriteOperator(ByteBlockBuffer byteBlockBuffer) : base(byteBlockBuffer)
        {
        }
        /// <summary>
        /// how many bytes you have already write in
        /// used to move postion
        /// </summary>
        /// <param name="length"></param>
        public void MovePosition(int length)
        {
            Position += length;
            if (Position >= ByteBlockBuffer.Size)
                throw new Exception("Writer position error");
            if (Position == ByteBlockBuffer.Size - 1) //current block is full
            {
                Position = 0;
                WriterNodeMove();
            }
        }
        void WriterNodeMove()
        {
            if (Node.Next == null) //next block is null
            {
                if (ByteBlockBuffer.LinkedList.First != ByteBlockBuffer.Reader.Node) //give priority to reuse block from beginning
                {
                    Node = ByteBlockBuffer.LinkedList.First;
                }
                else //create new block
                {
                    ByteBlockBuffer.LinkedList.AddAfter(Node, ByteBlockBuffer.GetBlock());
                    Node = Node.Next;
                }
            }
            else //next block is exist
            {
                if (Node.Next == ByteBlockBuffer.Reader.Node) //all blocks are occupied, create a new one
                {
                    ByteBlockBuffer.LinkedList.AddAfter(Node, ByteBlockBuffer.GetBlock());
                    Node = Node.Next;
                }
                else //reuse block
                {
                    Node = Node.Next;
                }
            }
        }
        public int GetHowManyCountCanWriteInThisBlock()
        {
            return ByteBlockBuffer.Size - Position;
        }
    }
}
