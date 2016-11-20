using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Task2.Logic
{
    public class Queue<T> : IEnumerable<T>, ICollection
    {
        private T[] arr;

        private int size;

        private int head;
        private int tail;

        /// <summary>
        /// Queue's capacity
        /// </summary>
        public int Capacity
        {
            get { return arr.Length; }
            private set
            {
                T[] temp = new T[value];
                Array.Copy(arr, head, temp, 0, size);
                arr = temp;
                head = 0;
                tail = size;
                //Array.Resize(ref arr, value);
            }
        }

        /// <summary>
        /// Ctor which initialize queue with this array
        /// </summary>
        /// <param name="source">array to initialize queue with</param>
        /// <exception cref="ArgumentNullException">Throws if source array is null</exception>
        public Queue(T[] source)
        {
            if (source == null)
                throw new ArgumentNullException($"{nameof(source)} is null");
            arr = new T[source.Length];
            source.CopyTo(arr, 0);
        }

        /// <summary>
        /// Create Queue with this capasity(default = 4)
        /// </summary>
        /// <param name="capacity">Capacity of generated Queue</param>
        public Queue(int capacity = 4)
        {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException($"Queue's capacity can't be negative");
            arr = new T[capacity];
        }

        /// <summary>
        /// Create queue from other collection
        /// </summary>
        /// <param name="collection">Collection to create queue from</param>
        public Queue(IEnumerable<T> collection)
        {
            if (collection == null)
                throw new ArgumentNullException($"Attempt to initialize Queue with null collection");
            IEnumerator<T> enumenator = collection.GetEnumerator();
            while (enumenator.MoveNext())
                Enqueue(enumenator.Current);
        }

        /// <summary>
        /// Delete first queue's element
        /// </summary>
        /// <returns>Deleted element</returns>
        /// <exception cref="InvalidOperationException">When queue is empty</exception>
        public T Dequeue()
        {
            if (size == 0)
                throw new InvalidOperationException("Queue is empty");
            size--;
            T local = arr[head];
            arr[head++] = default(T);
            return local;
        }

        /// <summary>
        /// Add element to queue
        /// </summary>
        /// <param name="element">Element to add</param>
        public void Enqueue(T element)
        {
            if (Capacity <= tail)
                Capacity = 2*size;

            size++;
            arr[tail++] = element;
        }

        /// <summary>
        /// Get first element from queue, but don't delete it 
        /// </summary>
        /// <returns>First element</returns>
        public T Peek()
        {
            if (IsEmpty())
                throw new InvalidOperationException("Queue is empty.");
            return arr[head];
        }

        /// <summary>
        /// Is queue empty?
        /// </summary>
        /// <returns>You really can't understand what does it return?</returns>
        public bool IsEmpty()
        {
            return size == 0;
        }

        /// <summary>
        /// Clear queue
        /// </summary>
        public void Clear()
        {
            Array.Clear(arr, 0, Capacity);
            size = 0;
            head = 0;
            tail = 0;
        }

        /// <summary>
        /// Get enumerator
        /// </summary>
        /// <returns>Enumerator</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return new CustomIterator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// It's private, you can't see it
        /// </summary>
        private struct CustomIterator : IEnumerator<T>
        {
            private readonly Queue<T> collection;
            private int currentIndex;

            public CustomIterator(Queue<T> collection)
            {
                currentIndex = collection.head - 1;
                this.collection = collection;
            }

            public T Current
            {
                get
                {
                    if (currentIndex == collection.head - 1 || currentIndex == collection.tail)
                    {
                        throw new InvalidOperationException();
                    }
                    return collection.arr[currentIndex];
                }
            }

            object IEnumerator.Current
            {
                get { throw new NotImplementedException(); }
            }

            public void Reset()
            {
                currentIndex = collection.head - 1;
                //throw new NotSupportedException();
            }

            public bool MoveNext()
            {
                return ++currentIndex < collection.tail;
            }

            public void Dispose()
            {
            }
        }

        #region different ICollection properties & methods
        /// <summary>
        /// Copy all elements from queue to Array
        /// </summary>
        /// <param name="array">Destination array</param>
        /// <param name="index">Index in array to start copying from</param>
        public void CopyTo(Array array, int index)
        {
            if (array == null)
                throw new ArgumentNullException($"{nameof(array)} is null");
            if (index < 0 || index >= array.Length)
                throw new ArgumentOutOfRangeException($"{nameof(index)} is out of range");
            if (array.Length - index < Count)
                throw new ArgumentException($"{nameof(array)} is too short");

            Array.Copy(arr, head, array, index, size);
        }
        public int Count => size;
        public object SyncRoot { get; } = new object();
        public bool IsSynchronized => false;
        #endregion
    }
}
