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
            WriteEqualRead, //init or read all data
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
            get
            {
                if (WritePosition > ReadPosition)
                    return State.WriteAhead;
                if (WritePosition < ReadPosition)
                    return State.ReadAhead;
                return State.WriteEqualRead;
            }
        }

        /// <summary>
        /// Index of read position(this index is wait to read)
        /// </summary>
        public int ReadPosition { get; private set; }

        /// <summary>
        /// Index of write postion（this index is wait to write）
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
                else if (EState == State.WriteEqualRead)
                {
                    remain = Size;
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
                else if (EState == State.WriteEqualRead)
                {
                    remain = 0;
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

            if (EState == State.WriteAhead)
            {
                var length = Size - WritePosition;
                if (length >= array.Length) //write into end
                {
                    for (int i = 0; i < array.Length; i++)
                    {
                        Array[WritePosition + i] = array[i];
                    }
                }
                else
                {
                    int arrayIndex = 0;
                    for (int i = WritePosition; i < Size; i++) //write into end 
                    {
                        Array[WritePosition + arrayIndex] = array[arrayIndex];
                        arrayIndex++;
                    }
                    var howManyAlreadWrite = arrayIndex; //alreay run arrayIndex++ so don't need +1
                    for (int i = 0; i < array.Length - howManyAlreadWrite; i++) //write into head
                    {
                        Array[i] = array[howManyAlreadWrite + i];
                        arrayIndex++;
                    }
                }
            }
            else if (EState == State.ReadAhead)
            {
                for (int i = 0; i < array.Length; i++)
                {
                    Array[WritePosition + i] = array[i];
                }
            }
            else if (EState == State.WriteEqualRead)
            {
                var length = Size - WritePosition;
                if (length >= array.Length) //write into end
                {
                    for (int i = 0; i < array.Length; i++)
                    {
                        Array[WritePosition + i] = array[i];
                    }
                }
                else
                {
                    int arrayIndex = 0;
                    for (int i = WritePosition; i < Size; i++) //write into end 
                    {
                        Array[i] = array[arrayIndex];
                        arrayIndex++;
                    }
                    var howManyAlreadWrite = arrayIndex; //alreay run arrayIndex++ so don't need +1
                    for (int i = 0; i < array.Length - howManyAlreadWrite; i++) //write into head
                    {
                        Array[i] = array[howManyAlreadWrite + i];
                        arrayIndex++;
                    }
                }
            }
            else
            {
                throw new Exception("Write error");
            }
            MoveWritePosition(array.Length);
        }

        public T[] Read(int length)
        {
            if (length > HowManyCanRead)
            {
                throw new Exception("Can not read so many data from array");
            }
            T[] array = new T[length];
            if (EState == State.WriteAhead)
            {
                for (int i = 0; i < length; i++)
                {
                    array[i] = Array[ReadPosition + i];
                }
            }
            else if (EState == State.ReadAhead)
            {
                var remainLength = Size - ReadPosition;
                if (remainLength >= length)
                {
                    for (int i = 0; i < length; i++)
                    {
                        array[i] = Array[ReadPosition + i];
                    }
                }
                else
                {
                    int arrayIndex = 0;
                    for (int i = ReadPosition; i < Size; i++)
                    {
                        array[arrayIndex] = Array[i];
                        arrayIndex++;
                    }
                    var howManyAlreadRead = arrayIndex; //alreay run arrayIndex++ so don't need +1
                    for (int i = 0; i < length - howManyAlreadRead; i++)
                    {
                        array[howManyAlreadRead + i] = Array[i];
                        arrayIndex++;
                    }
                }
            }
            else if (EState == State.WriteEqualRead) //write index back to read index
            {
                var remainLength = Size - ReadPosition;
                if (remainLength >= length)
                {
                    for (int i = 0; i < length; i++)
                    {
                        array[i] = Array[ReadPosition + i];
                    }
                }
                else
                {
                    int arrayIndex = 0;
                    for (int i = ReadPosition; i < Size; i++)
                    {
                        array[arrayIndex] = Array[i];
                        arrayIndex++;
                    }
                    var howManyAlreadRead = arrayIndex; //alreay run arrayIndex++ so don't need +1
                    for (int i = 0; i < length - howManyAlreadRead; i++)
                    {
                        array[howManyAlreadRead + i] = Array[i];
                        arrayIndex++;
                    }
                }
            }
            else
            {
                throw new Exception("read error");
            }
            MoveReadPosition(length);
            return array;
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            Array = null;
        }
    }
}