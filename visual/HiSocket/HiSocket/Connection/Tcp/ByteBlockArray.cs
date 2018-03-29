/****************************************************************************
 * Description:High-performance byte blocks
 * auto add block and auto reuse block
 * Author: hiramtan@live.com
 ****************************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace HiSocket
{
    class ByteBlockArray
    {
        public const int size = 1024;//block's size
        private LinkedList<byte[]> _linkedList = new LinkedList<byte[]>();

        public NodeInfo Reader { get; private set; }
        public NodeInfo Writer { get; private set; }

        public ByteBlockArray()
        {
            _linkedList.AddFirst(GetBlock());
            Reader = new NodeInfo(_linkedList.First, 0);
            Writer = new NodeInfo(_linkedList.First, 0);
        }
        /// <summary>
        /// how many bytes you have already write in
        /// </summary>
        /// <param name="length"></param>
        void Write(int length)
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
                else//create new block
                {
                    _linkedList.AddAfter(Writer.Node, GetBlock());
                    Writer.Node = Writer.Node.Next;
                }
            }
            else//next block is exist
            {
                if (Writer.Node.Next == Reader.Node)//all blocks are occupied, create a new one
                {
                    _linkedList.AddAfter(Writer.Node, GetBlock());
                    Writer.Node = Writer.Node.Next;
                }
                else//reuse block
                {
                    Writer.Node = Writer.Node.Next;
                }
            }
        }
        /// <summary>
        /// how many bytes you have read out
        /// </summary>
        /// <param name="length"></param>
        void ReadBytes(int length)
        {
            Reader.Position += length;
            if (Reader.Position >= size)
            { throw new Exception("Reader position error"); }
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
            if (Reader.Node.Next == null)//next block is null
            {
                Reader.Node = _linkedList.First;//read from first block
            }
            else//next block is not null
            {
                Reader.Node = Reader.Node.Next;
            }
        }

        public int GetHowManyCountWaitForRead()
        {
            if (Reader.Node == Writer.Node)// if there are in same block
                return Writer.Position - Reader.Position;
            return size - Reader.Position;//get rest of this block's bytes
        }

        bool IsReaderAndWriterInSameNode()
        {
            // if reader and writer in same block, reader's position must not large than writer's position
            return Reader.Node == Writer.Node;
        }

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