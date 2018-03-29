/****************************************************************************
 * Description:
 *
 * Author: hiramtan@live.com
 ****************************************************************************/
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HiSocket
{
    class Test
    {
        private const int size = 1024;//每个区块大小
        LinkedList<byte[]> linkedList = new LinkedList<byte[]>();

        private Handler reader;
        private Handler writer;
        void Init()
        {
            linkedList.AddFirst(GetBlock());
            reader = new Handler(linkedList.First, 0);
            writer = new Handler(linkedList.First, 0);
        }

        void Write(int length)
        {
            writer.Position += length;
            if (writer.Position > size - 1)
                Debug.LogError("writer position error");
            if (writer.Position == size - 1) //当前区块写满
            {
                WriteNodeMove();
            }
        }

        void WriteNodeMove()
        {
            if (writer._node.Next == null) //下一区块为null
            {
                if (linkedList.First != reader._node) //优先从头开始复用区块
                {
                    writer._node = linkedList.First;
                }
                else//创建新区块
                {
                    linkedList.AddAfter(writer._node, GetBlock());
                    writer._node = writer._node.Next;
                }
            }
            else//下一区块存在
            {
                if (writer._node.Next == reader._node)//所有区块被占满,创建新区块
                {
                    linkedList.AddAfter(writer._node, GetBlock());
                    writer._node = writer._node.Next;
                }
                else//复用区块
                {
                    writer._node = writer._node.Next;
                }
            }
        }

        void ReadBytes()
        {
        }


        byte[] GetBlock()
        {
            return new byte[size];
        }
    }

    public class Handler
    {

        public LinkedListNode<byte[]> _node;
        public int Position;


        public Handler(LinkedListNode<byte[]> node, int position)
        {
            Position = position;
            _node = node;
        }
    }
}
