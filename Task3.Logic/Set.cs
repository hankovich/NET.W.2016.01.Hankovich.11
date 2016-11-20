using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Task3.Logic
{
    public class Set<T> : ICollection<T> where T : class, IEquatable<T>
    {
        private T[] arr;
        private int size;

        public int Count => size;
 
        public bool IsReadOnly => false;

        /// <summary>
        /// Generate set from array
        /// </summary>
        /// <param name="source">Array to generate set from</param>
        /// <exception cref="ArgumentNullException">Throws when source is null</exception>
        public Set(params T[] source)
        {
            if (source == null)
                throw new ArgumentNullException($"{nameof(source)} is null");
            arr = new T[source.Length];
            source.CopyTo(arr, 0);
            size = source.Length;
        }

        /// <summary>
        /// Create Set with this capasity(default = 4)
        /// </summary>
        /// <param name="capacity">Capacity of generated Set</param>
        /// <exception cref="ArgumentOutOfRangeException">Throws when capacity is negative</exception>
        public Set(int capacity = 4)
        {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException("Set's capacity can't be negative");
            arr = new T[capacity];
        }

        /// <summary>
        /// Create set from other collection
        /// </summary>
        /// <param name="collection">Collection to create queue from</param>
        /// <exception cref="ArgumentNullException">When collection is null</exception>
        public Set(IEnumerable<T> collection)
        {
            if (collection == null)
                throw new ArgumentNullException("Attempt to initialize Set with null collection");
            IEnumerator<T> enumenator = collection.GetEnumerator();
            while (enumenator.MoveNext())
                Add(enumenator.Current);
        }

        /// <summary>
        /// Add item to set
        /// </summary>
        /// <param name="item">Item to add</param>
        public void Add(T item)
        {
            if (arr.Length >= size)
                Array.Resize(ref arr, 2*arr.Length);
            arr[size++] = item;
        }

        /// <summary>
        /// Union two IEnumerables
        /// </summary>
        /// <param name="other">IEnumerable to union with</param>
        /// <returns>Union of this and other</returns>
        /// <exception cref="ArgumentNullException">Throws when other is null</exception>
        public IEnumerable<T> Union(IEnumerable<T> other)
        {
            if (other == null)
                throw new ArgumentNullException($"{nameof(other)} is null");
            return GetSetWithoutRemovedElements().Union(other is Set<T> ? ((Set<T>)other).GetSetWithoutRemovedElements() : other);
        }

        /// <summary>
        /// Intersect two IEnumerables
        /// </summary>
        /// <param name="other">IEnumerable to intersect with</param>
        /// <returns>Intersect of this and other</returns>
        /// <exception cref="ArgumentNullException">Throws when other is null</exception>
        public IEnumerable<T> Intersect(IEnumerable<T> other)
        {
            if (other == null)
                throw new ArgumentNullException($"{nameof(other)} is null");
            return GetSetWithoutRemovedElements().Intersect(other is Set<T> ? ((Set<T>)other).GetSetWithoutRemovedElements() : other);
        }

        /// <summary>
        /// Except two IEnumerables
        /// </summary>
        /// <param name="other">IEnumerable to except with</param>
        /// <returns>Except of this and other</returns>
        /// <exception cref="ArgumentNullException">Throws when other is null</exception>
        public IEnumerable<T> Except(IEnumerable<T> other)
        {
            if (other == null)
                throw new ArgumentNullException($"{nameof(other)} is null");
            return GetSetWithoutRemovedElements().Except(other is Set<T> ? ((Set<T>)other).GetSetWithoutRemovedElements() : other);
        }

        /// <summary>
        /// Check whether the current set and the specified collection intersect.
        /// </summary>
        /// <param name="other">Operand</param>
        /// <exception cref="ArgumentNullException">Throws when other is null</exception>
        public bool Overlaps(IEnumerable<T> other)
        {
            if (other == null) throw new ArgumentNullException($"{nameof(other)} is null");
            return (other is Set<T> ? ((Set<T>)other).GetSetWithoutRemovedElements() : other).Any(i => GetSetWithoutRemovedElements().Contains(i));
        }

        /// <summary>
        /// Check the equatily of two sets
        /// </summary>
        /// <param name="other">Second set</param>
        /// <returns>True if sets are equal and false, otherwise</returns>
        /// <exception cref="ArgumentNullException">Throws when other is null</exception>
        public bool SetEquals(IEnumerable<T> other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));
            return GetSetWithoutRemovedElements().SequenceEqual(other is Set<T> ? ((Set<T>) other).GetSetWithoutRemovedElements() : other);
        }

        /// <summary>
        /// Clear set
        /// </summary>
        public void Clear()
        {
            Array.Clear(arr, 0, arr.Length);
            size = 0;
        }

        /// <summary>
        /// Check the presence of the element in the set
        /// </summary>
        /// <param name="item">Element to check</param>
        /// <returns>True, if set contains item, false, otherwise</returns>
        public bool Contains(T item)
        {
            return GetSetWithoutRemovedElements().Contains(item);
        }

        /// <summary>
        /// Copy elements from Set to array and begin coping from current index of the array.
        /// </summary>
        /// <param name="array">Array where need to copy</param>
        /// <param name="arrayIndex">Index to start copy from</param>
        /// <exception cref="ArgumentOutOfRangeException">Throws when arrayIndex is more than array capacity</exception>
        /// <exception cref="ArgumentNullException">Throws when array is null</exception>
        /// <exception cref="ArgumentException">Throws when array is too short to copy all elements from set</exception>
        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null) throw new ArgumentNullException($"{nameof(array)} is null");
            if (array.Length <= arrayIndex) throw new ArgumentOutOfRangeException($"{nameof(arrayIndex)} is more than {nameof(array)} length");
            if (array.Length - arrayIndex < size) throw new ArgumentException($"{nameof(array)} is too short");
            Array.Copy(arr, 0, array, arrayIndex, size);
        }

        public IEnumerator<T> GetEnumerator() => GetSetWithoutRemovedElements().GetEnumerator();

        /// <summary>
        /// Remove element from Set
        /// </summary>
        /// <param name="item">Element to remove</param>
        /// <returns>True if removed and false if not(thete aren't such element in set)</returns>
        public bool Remove(T item)
        {
            int index = Array.IndexOf(arr, item);
            if (index != -1)
            {
                for (int i = index; i < size; i++)
                {
                    arr[i] = arr[i + 1];
                }
                arr[size] = default(T);
                size--;
            }
            return index != -1;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetSetWithoutRemovedElements().GetEnumerator();

        private IEnumerable<T> GetSetWithoutRemovedElements()
        {
            T[] temp = new T[size];
            Array.Copy(arr, 0, temp, 0, size);
            return temp;
        }
    }
}
