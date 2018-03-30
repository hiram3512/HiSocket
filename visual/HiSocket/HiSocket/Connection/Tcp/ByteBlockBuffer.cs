/****************************************************************************
 * Description:High-performance byte blocks
 * auto add block and auto reuse block
 * you can directly operate reader or writer's byte array
 * or you can use api: readall()/writeall() to read or write whole bytes in diffrent block
 * Author: hiramtan@live.com
 ****************************************************************************/

using System;
using System.Collections.Generic;

namespace HiSocket
{
    public class ByteBlockBuffer
    {
        public const int size = 1024; //block's size
        private LinkedList<byte[]> _linkedList = new LinkedList<byte[]>();

        public NodeInfo Reader { get; private set; }
        public NodeInfo Writer { get; private set; }

        public ByteBlockBuffer()
        {
            _linkedList.AddFirst(GetBlock());
            Reader = new NodeInfo(_linkedList.First, 0);
            Writer = new NodeInfo(_linkedList.First, 0);
        }

        /// <summary>
        /// how many bytes you have already write in
        /// used to move postion
        /// </summary>
        /// <param name="length"></param>
        public void WriteInThisBlock(int length)
        {
            Writer.Position += length;
            if (Writer.Position >= size)
                throw new Exception("Writer position error");
            if (Writer.Position == size - 1) //current block is full
            {
                Writer.Position = 0;
                WriterNodeMove();
            }
        }

        void WriterNodeMove()
        {
            if (Writer.Node.Next == null) //next block is null
            {
                if (_linkedList.First != Reader.Node) //give priority to reuse block from beginning
                {
                    Writer.Node = _linkedList.First;
                }
                else //create new block
                {
                    _linkedList.AddAfter(Writer.Node, GetBlock());
                    Writer.Node = Writer.Node.Next;
                }
            }
            else //next block is exist
            {
                if (Writer.Node.Next == Reader.Node) //all blocks are occupied, create a new one
                {
                    _linkedList.AddAfter(Writer.Node, GetBlock());
                    Writer.Node = Writer.Node.Next;
                }
                else //reuse block
                {
                    Writer.Node = Writer.Node.Next;
                }
            }
        }

        /// <summary>
        /// how many bytes you have read out
        /// used to move postion
        /// </summary>
        /// <param name="length"></param>
        public void ReadInThisBlock(int length)
        {
            Reader.Position += length;
            if (Reader.Position >= size)
            {
                throw new Exception("Reader position error");
            }
            if (IsReaderAndWriterInSameNode())
            {
                if (Reader.Position > Writer.Position)
                {
                    throw new Exception("When in same node, Reader's position must not large than Writer's postion");
                }
            }
            if (Reader.Position == size - 1) //current block have already read finish
            {
                Reader.Position = 0;
                ReaderNoderMove();
            }
        }

        void ReaderNoderMove()
        {
            if (Reader.Node.Next == null) //next block is null
            {
                Reader.Node = _linkedList.First; //read from first block
            }
            else //next block is not null
            {
                Reader.Node = Reader.Node.Next;
            }
        }

        public int GetHowManyCountCanReadInThisBlock()
        {
            if (Reader.Node == Writer.Node) // if there are in same block
                return Writer.Position - Reader.Position;
            return size - Reader.Position; //get rest of this block's bytes
        }

        public int GetHowManyCountCanWriteInThisBlock()
        {
            return size - Writer.Position;
        }

        bool IsReaderAndWriterInSameNode()
        {
            // if reader and writer in same block, reader's position must not large than writer's position
            return Reader.Node == Writer.Node;
        }
        #region operate all blocks

        public byte[] ReadAllBytes()
        {
            if (IsReaderAndWriterInSameNode())
            {
                var length = Writer.Position - Reader.Position;
                var bytes = new byte[length];
                Array.Copy(Reader.Node.Value, Reader.Position, bytes, 0, length);
                ReadInThisBlock(length);
                return bytes;
            }
            else
            {
                var readerBlockBytes = new byte[size - Reader.Position];
                Array.Copy(Reader.Node.Value, Reader.Position, readerBlockBytes, 0, readerBlockBytes.Length);
                var betweenReadAndWriterBytes = new List<byte>();
                GetBytesBetweenThoseTwo(Reader.Node, Writer.Node, ref betweenReadAndWriterBytes);
                var writerBlockBytes = new byte[Writer.Position];
                Array.Copy(Writer.Node.Value, 0, writerBlockBytes, 0, writerBlockBytes.Length);
                List<byte> bytes = new List<byte>();
                bytes.AddRange(readerBlockBytes);
                bytes.AddRange(betweenReadAndWriterBytes);
                bytes.AddRange(writerBlockBytes);
                return bytes.ToArray();
            }
        }

        // haven't use for current now
        //void GetHowManyBlockCountBetweenThoseTwo(LinkedListNode<byte[]> first, LinkedListNode<byte[]> last, ref int count)
        //{
        //    if (first == last)
        //    {
        //        throw new Exception("first node and last node is same");
        //    }
        //    else if (first.Next == last)
        //    {
        //        //0 node between this two blocks
        //        count += 0;
        //    }
        //    else
        //    {
        //        count += 1;
        //        GetHowManyBlockCountBetweenThoseTwo(first.Next, last, ref count);
        //    }
        //}

        void GetBytesBetweenThoseTwo(LinkedListNode<byte[]> first, LinkedListNode<byte[]> last, ref List<byte> bytes)
        {
            if (first == last)
            {
                throw new Exception("first node and last node is same");
            }
            else if (first.Next == last)
            {
                //0 node between this two blocks
            }
            else
            {
                bytes.AddRange(first.Next.Value);
                GetBytesBetweenThoseTwo(first.Next, last, ref bytes);
            }
        }

        public void WriteAllBytes()
        {

        }
        #endregion
        byte[] GetBlock()
        {
            return new byte[size];
        }
        public class NodeInfo
        {
            public LinkedListNode<byte[]> Node;
            public int Position;
            public NodeInfo(LinkedListNode<byte[]> node, int position)
            {
                Position = position;
                Node = node;
            }
        }
    }
}