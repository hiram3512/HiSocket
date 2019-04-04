/***************************************************************
 * Description:High-performance byte blocks
 * auto add block and auto reuse block
 * you can directly operate reader or writer's byte array
 * or you can use api: readall()/writeall() to read or write whole bytes in diffrent block
 *
 * Documents: https://github.com/hiramtan/HiSocket
 * Author: hiramtan@live.com
***************************************************************/

using System;
using System.Collections.Generic;

namespace HiSocket
{
    internal sealed class ByteBlockBuffer : IByteBlockBuffer
    {
        public int Size { get; }
        public int Count { get; private set; }
        public LinkedList<byte[]> LinkedList { get; private set; }
        public ReadOperator Reader { get; private set; }
        public WriteOperator Writer { get; private set; }

        /// <summary>
        /// user may use thread to process bytes
        /// </summary>
        private readonly object locker = new object();
        public ByteBlockBuffer(int size = 1 << 16)
        {
            Size = size;
            LinkedList = new LinkedList<byte[]>();
            LinkedList.AddFirst(CreateBlock());
            Reader = new ReadOperator(this);
            Writer = new WriteOperator(this);
        }
        public bool IsReaderAndWriterInSameNode()
        {
            lock (locker)
            {
                // if reader and writer in same block, reader's position must not large than writer's position
                return Reader.Node == Writer.Node;
            }
        }
        #region operate all blocks

        public byte[] ReadAllBytes()
        {
            lock (locker)
            {
                if (IsReaderAndWriterInSameNode())
                {
                    var length = Writer.Position - Reader.Position;
                    var bytes = new byte[length];
                    Array.Copy(Reader.Node.Value, Reader.Position, bytes, 0, length);
                    Reader.MovePosition(length);
                    return bytes;
                }
                else
                {
                    var readerBlockBytes = new byte[Size - Reader.Position];
                    Array.Copy(Reader.Node.Value, Reader.Position, readerBlockBytes, 0, readerBlockBytes.Length);
                    var betweenReadAndWriterBytes = new List<byte>();
                    GetBytesBetweenReaderAndWriter(Reader.Node, Writer.Node, ref betweenReadAndWriterBytes);
                    var writerBlockBytes = new byte[Writer.Position];
                    Array.Copy(Writer.Node.Value, 0, writerBlockBytes, 0, writerBlockBytes.Length);
                    List<byte> bytes = new List<byte>();
                    bytes.AddRange(readerBlockBytes);
                    bytes.AddRange(betweenReadAndWriterBytes);
                    bytes.AddRange(writerBlockBytes);
                    Reader.Node = Writer.Node;//finish and move reader
                    Reader.Position = 0;
                    Reader.MovePosition(Writer.Position);
                    return bytes.ToArray();
                }
            }
        }
        void GetBytesBetweenReaderAndWriter(LinkedListNode<byte[]> reader, LinkedListNode<byte[]> writer, ref List<byte> bytes)
        {
            if (reader.Next == null)
            {
                //when writer reuse and at begin reader at end, this will hapen
                reader = LinkedList.First;
                if (reader != writer)
                {
                    bytes.AddRange(reader.Value);
                    GetBytesBetweenReaderAndWriter(reader, writer, ref bytes);
                }
            }
            else if (reader.Next == writer)
            {
                //0 node between this two blocks
            }
            else
            {
                reader = reader.Next;
                bytes.AddRange(reader.Value);
                GetBytesBetweenReaderAndWriter(reader, writer, ref bytes);
            }
        }
        public void WriteAllBytes(byte[] bytes)
        {
            lock (locker)
            {
                WriteAllBytes(bytes, 0);
            }
        }

        /// <summary>
        /// write all bytes
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="index">how many bytes already been write in</param>
        void WriteAllBytes(byte[] bytes, int index)
        {
            if (index < bytes.Length)
            {
                var canWriteLength = Writer.GetHowManyCountCanWriteInThisBlock();
                var haventWriteLength = bytes.Length - index;
                var reallyWriteLength = haventWriteLength < canWriteLength ? haventWriteLength : canWriteLength;
                Array.Copy(bytes, index, Writer.Node.Value, Writer.Position, reallyWriteLength);
                index += reallyWriteLength;
                Writer.MovePosition(reallyWriteLength);
                WriteAllBytes(bytes, index);
            }
        }
        #endregion
        public byte[] CreateBlock()
        {
            lock (locker)
            {
                Count += 1;
                return new byte[Size];
            }
        }
    }
}