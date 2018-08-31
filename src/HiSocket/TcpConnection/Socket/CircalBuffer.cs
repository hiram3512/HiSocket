/****************************************************************************
 * Description:
 *
 * Author: hiramtan@live.com
 ****************************************************************************/

using System;

namespace HiFramework
{
    /// <summary>
    /// This is a circular buffer class for reuse memory
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CircularBuffer<T> : ICircularBuffer<T>
    {
        /// <summary>
        /// Read and write state
        /// </summary>
        public enum State
        {
            WriteAhead,
            ReadAhead,
        }

        /// <summary>
        /// Array to contain element
        /// </summary>
        public T[] Array { get; private set; }

        /// <summary>
        /// Capacity
        /// </summary>
        public int Size { get; private set; }

        /// <summary>
        /// Current read and write state
        /// </summary>
        public State EState
        {
            get { return WritePosition >= ReadPosition ? State.WriteAhead : State.ReadAhead; }
        }

        /// <summary>
        /// Index of read position
        /// </summary>
        public int ReadPosition { get; private set; }

        /// <summary>
        /// Index of write postion
        /// </summary>
        public int WritePosition { get; private set; }

        /// <summary>
        /// how many data can write into array
        /// </summary>
        public int HowManyCanWrite
        {
            get
            {
                int remain = 0;
                if (EState == State.WriteAhead)
                {
                    remain = (Size - WritePosition) + ReadPosition;
                }
                else if (EState == State.ReadAhead) //writer back to head
                {
                    remain = ReadPosition - WritePosition;
                }
                return remain;
            }
        }

        /// <summary>
        /// How many data wait read
        /// </summary>
        public int HowManyCanRead
        {
            get
            {
                int remain = 0;
                if (EState == State.WriteAhead)
                {
                    remain = WritePosition - ReadPosition;
                }
                else if (EState == State.ReadAhead) //writer back to head
                {
                    remain = (Size - ReadPosition) + WritePosition;
                }
                return remain;
            }
        }

        public CircularBuffer(int size = 2 << 13)
        {
            Size = size;
            Array = new T[Size];
        }

        /// <summary>
        /// Move read index
        /// </summary>
        /// <param name="length"></param>
        public void MoveReadPosition(int length)
        {
            if (length > Size)
            {
                throw new ArgumentOutOfRangeException("Length is large than array's capacity");
            }
            if (length > HowManyCanRead)
            {
                throw new ArgumentOutOfRangeException("Read length large than data's length");
            }
            var index = ReadPosition + length;
            if (index >= Size)
            {
                index -= Size;
            }
            ReadPosition = index;
        }

        /// <summary>
        /// Move write index
        /// </summary>
        /// <param name="length"></param>
        public void MoveWritePosition(int length)
        {
            if (length > Size)
            {
                throw new Exception("Length is large than array's capacity");
            }
            if (length > HowManyCanWrite)
            {
                throw new Exception("Write length large than space");
            }
            var index = WritePosition + length;
            if (index >= Size)
            {
                index -= Size;
            }
            WritePosition = index;
        }

        /// <summary>
        /// Write data to array
        /// </summary>
        /// <param name="array"></param>
        public void Write(T[] array)
        {
            if (array.Length > HowManyCanWrite)
            {
                throw new Exception("Can not write so many data to array");
            }
            var length = Size - WritePosition;
            if (length >= array.Length)//write into end
            {
                for (int i = 0; i < array.Length; i++)
                {
                    Array[WritePosition + i] = array[i];
                }
            }
            else
            {
                for (int i = WritePosition; i < Size; i++)//write into end 
                {
                    Array[WritePosition + i] = array[i];
                }
                var howManyAlreadWrite = Size - WritePosition;
                for (int i = 0; i < array.Length - howManyAlreadWrite; i++)//write into head
                {
                    Array[i] = array[howManyAlreadWrite + i];
                }
            }
            MoveWritePosition(array.Length);
        }

        /// <summary>
        /// Read all from array
        /// </summary>
        /// <returns></returns>
        public T[] ReadAll()
        {
            T[] ts = new T[HowManyCanRead];
            if (EState == State.WriteAhead)
            {
                for (int i = 0; i < HowManyCanRead; i++)//read end
                {
                    ts[i] = Array[ReadPosition + i];
                }
            }
            else if (EState == State.ReadAhead)
            {
                var length = Size - ReadPosition;
                for (int i = 0; i < length; i++)//read end
                {
                    ts[i] = Array[ReadPosition + i];
                }
                for (int i = 0; i < WritePosition; i++)//read head
                {
                    ts[length + i] = Array[i];
                }
            }
            MoveReadPosition(ts.Length);
            return ts;
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            Array = null;
        }
    }
}
