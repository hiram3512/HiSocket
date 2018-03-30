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
        public int Size { get; private set; }//block's size
        public LinkedList<byte[]> LinkedList { get; private set; }

        public ReadOperater Reader { get; private set; }
        public WriteOperater Writer { get; private set; }

        public ByteBlockBuffer(int size = 1024)
        {
            Size = size;
            LinkedList = new LinkedList<byte[]>();
            LinkedList.AddFirst(GetBlock());
            Reader = new ReadOperater(this);
            Writer = new WriteOperater(this);
        }

        /// <summary>
        /// how many bytes you have read out
        /// used to move postion
        /// </summary>
        /// <param name="length"></param>
        public void ReadInThisBlock(int length)
        {
            Reader.Position += length;
            if (Reader.Position >= Size)
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
            if (Reader.Position == Size - 1) //current block have already read finish
            {
                Reader.Position = 0;
                ReaderNoderMove();
            }
        }

        void ReaderNoderMove()
        {
            if (Reader.Node.Next == null) //next block is null
            {
                Reader.Node = LinkedList.First; //read from first block
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
            return Size - Reader.Position; //get rest of this block's bytes
        }

        public int GetHowManyCountCanWriteInThisBlock()
        {
            return Size - Writer.Position;
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
                var readerBlockBytes = new byte[Size - Reader.Position];
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



        public void WriteAllBytes(byte[] bytes)
        {
            WriteAllBytes(bytes, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="index">how many bytes already been write in</param>
        void WriteAllBytes(byte[] bytes, int index)
        {
            //if (index < bytes.Length)
            //{
            //    var length = GetHowManyCountCanWriteInThisBlock();
            //    Array.Copy(bytes, index, Writer.Node.Value, Writer.Position, length);
            //    index += length;
            //    MovePosition(length);
            //    WriteAllBytes(bytes, index);
            //}
        }
        #endregion
        public byte[] GetBlock()
        {
            return new byte[Size];
        }
        public abstract class NodeOperater
        {
            protected ByteBlockBuffer ByteBlockBuffer { get; }
            public LinkedListNode<byte[]> Node { get; set; }
            public int Position { get; set; }
            public NodeOperater(ByteBlockBuffer byteBlockBuffer)
            {
                ByteBlockBuffer = byteBlockBuffer;
                Node = ByteBlockBuffer.LinkedList.First;
            }
        }

        public class ReadOperater : NodeOperater
        {
            public ReadOperater(ByteBlockBuffer byteBlockBuffer) : base(byteBlockBuffer)
            {
            }
        }

        public class WriteOperater : NodeOperater
        {
            public WriteOperater(ByteBlockBuffer byteBlockBuffer) : base(byteBlockBuffer)
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
        }
    }
}