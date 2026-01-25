// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Buffers;
using System.Collections;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace Faster.Collections.Pooled;

/// <summary>
/// Represents a first-in, first-out collection of objects.
/// </summary>
/// <remarks>
/// Implemented as a circular buffer, so <see cref="Enqueue(T)"/> and <see cref="Dequeue"/> are typically <c>O(1)</c>.
/// </remarks>
[DebuggerTypeProxy(typeof(QueueDebugView<>))]
[DebuggerDisplay("Count = {Count}")]
[Serializable]
public partial class PooledQueue<T> : IEnumerable<T>,
    ICollection,
    IReadOnlyCollection<T>,
    IDisposable
{
    private T[] _array;
    private int _head;       // The index from which to dequeue if the queue isn't empty.
    private int _tail;       // The index at which to enqueue if the queue isn't full.
    private int _size;       // Number of elements.
    private int _version;

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected virtual T[] Alloc(int size)
    {
        return SingleThreadedArrayPool<T>.Shared.Rent(size);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected virtual void Free(T[] array)
    {
        if (array.Length != 0)
            SingleThreadedArrayPool<T>.Shared.Return(array, RuntimeHelpers.IsReferenceOrContainsReferences<T>());
    }

    // Creates a queue with room for capacity objects. The default initial
    // capacity and grow factor are used.
    public PooledQueue()
    {
        _array = Array.Empty<T>();
    }

    // Creates a queue with room for capacity objects. The default grow factor
    // is used.
    public PooledQueue(int capacity)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(capacity);
        if (capacity == 0)
        {
            _array = Array.Empty<T>();
        }
        else
        {
            _array = Alloc(capacity);
        }
    }

    // Fills a Queue with the elements of an ICollection.  Uses the enumerator
    // to get each of the elements.
    public PooledQueue(IEnumerable<T> collection)
    {
        ArgumentNullException.ThrowIfNull(collection);

        var pooled = EnumerableHelpers.ToPooledArray(collection, out _size);
        _array = Alloc(_size);
        Array.Copy(pooled, _array, _size);
        if (pooled.Length != 0)
            ArrayPool<T>.Shared.Return(pooled, clearArray: RuntimeHelpers.IsReferenceOrContainsReferences<T>());
        
        if (_size != _array.Length) _tail = _size;
    }

    public int Count => _size;

    /// <summary>
    /// Gets the total numbers of elements the internal data structure can hold without resizing.
    /// </summary>
    public int Capacity => _array.Length;

    /// <inheritdoc cref="ICollection.IsSynchronized" />
    bool ICollection.IsSynchronized => false;

    object ICollection.SyncRoot => this;

    // Removes all Objects from the queue.
    public void Clear()
    {
        if (_size != 0)
        {
            if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
            {
                if (_head < _tail)
                {
                    Array.Clear(_array, _head, _size);
                }
                else
                {
                    Array.Clear(_array, _head, _array.Length - _head);
                    Array.Clear(_array, 0, _tail);
                }
            }

            _size = 0;
        }

        _head = 0;
        _tail = 0;
        _version++;
    }

    // CopyTo copies a collection into an Array, starting at a particular
    // index into the array.
    public void CopyTo(T[] array, int arrayIndex)
    {
        ArgumentNullException.ThrowIfNull(array);

        if (arrayIndex < 0 || arrayIndex > array.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(arrayIndex));
        }

        if (array.Length - arrayIndex < _size)
        {
            throw new ArgumentException(nameof(array));
        }

        int numToCopy = _size;
        if (numToCopy == 0) return;

        int firstPart = Math.Min(_array.Length - _head, numToCopy);
        Array.Copy(_array, _head, array, arrayIndex, firstPart);
        numToCopy -= firstPart;
        if (numToCopy > 0)
        {
            Array.Copy(_array, 0, array, arrayIndex + _array.Length - _head, numToCopy);
        }
    }

    void ICollection.CopyTo(Array array, int index)
    {
        ArgumentNullException.ThrowIfNull(array);

        if (array.Rank != 1)
        {
            throw new ArgumentException(nameof(array));
        }

        if (array.GetLowerBound(0) != 0)
        {
            throw new ArgumentException(nameof(array));
        }

        int arrayLen = array.Length;
        if (index < 0 || index > arrayLen)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }

        if (arrayLen - index < _size)
        {
            throw new ArgumentException(nameof(array));
        }

        int numToCopy = _size;
        if (numToCopy == 0) return;

        try
        {
            int firstPart = (_array.Length - _head < numToCopy) ? _array.Length - _head : numToCopy;
            Array.Copy(_array, _head, array, index, firstPart);
            numToCopy -= firstPart;

            if (numToCopy > 0)
            {
                Array.Copy(_array, 0, array, index + _array.Length - _head, numToCopy);
            }
        }
        catch (ArrayTypeMismatchException)
        {
            throw new ArgumentException(nameof(array));
        }
    }

    // Adds item to the tail of the queue.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Enqueue(T item)
    {
        if (_size == _array.Length)
        {
            Grow(_size + 1);
        }

        _array[_tail] = item;
        MoveNext(ref _tail);
        _size++;
        _version++;
    }

    // GetEnumerator returns an IEnumerator over this Queue.  This
    // Enumerator will support removing.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Enumerator GetEnumerator() => new Enumerator(this);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    /// <internalonly/>
    IEnumerator<T> IEnumerable<T>.GetEnumerator() =>
        Count == 0 ? SZGenericArrayEnumerator<T>.Empty :
        GetEnumerator();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<T>)this).GetEnumerator();

    // Removes the object at the head of the queue and returns it. If the queue
    // is empty, this method throws an
    // InvalidOperationException.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T Dequeue()
    {
        int head = _head;
        T[] array = _array;

        if (_size == 0)
        {
            ThrowForEmptyQueue();
        }

        T removed = array[head];
        if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
        {
            array[head] = default!;
        }
        MoveNext(ref _head);
        _size--;
        _version++;
        return removed;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryDequeue([MaybeNullWhen(false)] out T result)
    {
        int head = _head;
        T[] array = _array;

        if (_size == 0)
        {
            result = default;
            return false;
        }

        result = array[head];
        if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
        {
            array[head] = default!;
        }
        MoveNext(ref _head);
        _size--;
        _version++;
        return true;
    }

    // Returns the object at the head of the queue. The object remains in the
    // queue. If the queue is empty, this method throws an
    // InvalidOperationException.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T Peek()
    {
        if (_size == 0)
        {
            ThrowForEmptyQueue();
        }

        return _array[_head];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryPeek([MaybeNullWhen(false)] out T result)
    {
        if (_size == 0)
        {
            result = default;
            return false;
        }

        result = _array[_head];
        return true;
    }

    // Returns true if the queue contains at least one object equal to item.
    // Equality is determined using EqualityComparer<T>.Default.Equals().
    public bool Contains(T item)
    {
        if (_size == 0)
        {
            return false;
        }

        if (_head < _tail)
        {
            return Array.IndexOf(_array, item, _head, _size) >= 0;
        }

        // We've wrapped around. Check both partitions, the least recently enqueued first.
        return
            Array.IndexOf(_array, item, _head, _array.Length - _head) >= 0 ||
            Array.IndexOf(_array, item, 0, _tail) >= 0;
    }

    // Iterates over the objects in the queue, returning an array of the
    // objects in the Queue, or an empty array if the queue is empty.
    // The order of elements in the array is first in to last in, the same
    // order produced by successive calls to Dequeue.
    public T[] ToArray()
    {
        if (_size == 0)
        {
            return Array.Empty<T>();
        }

        T[] arr = new T[_size];

        if (_head < _tail)
        {
            Array.Copy(_array, _head, arr, 0, _size);
        }
        else
        {
            Array.Copy(_array, _head, arr, 0, _array.Length - _head);
            Array.Copy(_array, 0, arr, _array.Length - _head, _tail);
        }

        return arr;
    }

    // PRIVATE Grows or shrinks the buffer to hold capacity objects. Capacity
    // must be >= _size.
    private void SetCapacity(int capacity)
    {
        Debug.Assert(capacity >= _size);
        T[] newarray = Alloc(capacity);
        if (_size > 0)
        {
            if (_head < _tail)
            {
                Array.Copy(_array, _head, newarray, 0, _size);
            }
            else
            {
                Array.Copy(_array, _head, newarray, 0, _array.Length - _head);
                Array.Copy(_array, 0, newarray, _array.Length - _head, _tail);
            }
        }
        
        Free(_array);
        _array = newarray;
        _head = 0;
        _tail = (_size == capacity) ? 0 : _size;
        _version++;
    }

    // Increments the index wrapping it if necessary.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void MoveNext(ref int index)
    {
        // It is tempting to use the remainder operator here but it is actually much slower
        // than a simple comparison and a rarely taken branch.
        // JIT produces better code than with ternary operator ?:
        int tmp = index + 1;
        if (tmp == _array.Length)
        {
            tmp = 0;
        }
        index = tmp;
    }

    private void ThrowForEmptyQueue()
    {
        Debug.Assert(_size == 0);
        throw new InvalidOperationException($"Queue {this} is empty.");
    }

    public void TrimExcess()
    {
        int threshold = (int)(_array.Length * 0.9);
        if (_size < threshold)
        {
            SetCapacity(_size);
        }
    }

    /// <summary>
    /// Sets the capacity of a <see cref="PooledQueue{T}"/> object to the specified number of entries.
    /// </summary>
    /// <param name="capacity">The new capacity.</param>
    /// <exception cref="ArgumentOutOfRangeException">Passed capacity is lower than entries count.</exception>
    public void TrimExcess(int capacity)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(capacity);
        ArgumentOutOfRangeException.ThrowIfLessThan(capacity, _size);

        if (capacity == _array.Length)
            return;

        SetCapacity(capacity);
    }

    /// <summary>
    /// Ensures that the capacity of this Queue is at least the specified <paramref name="capacity"/>.
    /// </summary>
    /// <param name="capacity">The minimum capacity to ensure.</param>
    /// <returns>The new capacity of this queue.</returns>
    public int EnsureCapacity(int capacity)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(capacity);

        if (_array.Length < capacity)
        {
            Grow(capacity);
        }

        return _array.Length;
    }

    private void Grow(int capacity)
    {
        Debug.Assert(_array.Length < capacity);

        const int GrowFactor = 2;
        const int MinimumGrow = 4;

        int newCapacity = GrowFactor * _array.Length;

        // Allow the list to grow to maximum possible capacity (~2G elements) before encountering overflow.
        // Note that this check works even when _items.Length overflowed thanks to the (uint) cast
        if ((uint)newCapacity > Array.MaxLength) newCapacity = Array.MaxLength;

        // Ensure minimum growth is respected.
        newCapacity = Math.Max(newCapacity, _array.Length + MinimumGrow);

        // If the computed capacity is still less than specified, set to the original argument.
        // Capacities exceeding Array.MaxLength will be surfaced as OutOfMemoryException by Array.Resize.
        if (newCapacity < capacity) newCapacity = capacity;

        SetCapacity(newCapacity);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            Free(_array);
            _array = Array.Empty<T>();
            _size = 0;
            _head = 0;
            _tail = 0;
            _version++;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    // Implements an enumerator for a Queue.  The enumerator uses the
    // internal version number of the list to ensure that no modifications are
    // made to the list while an enumeration is in progress.
    public struct Enumerator : IEnumerator<T>,
        IEnumerator
    {
        private readonly PooledQueue<T> _queue;
        private readonly int _version;
        private int _i;
        private T? _currentElement;

        internal Enumerator(PooledQueue<T> queue)
        {
            _queue = queue;
            _version = queue._version;
            _i = -1;
            _currentElement = default;
        }

        public void Dispose()
        {
            _i = -2;
            _currentElement = default;
        }

        public bool MoveNext()
        {
            if (_version != _queue._version)
            {
                throw new InvalidOperationException($"Queue {this} was modified during enumeration.");
            }

            PooledQueue<T> q = _queue;
            int size = q._size;

            int offset = _i + 1;
            if ((uint)offset < (uint)size)
            {
                _i = offset;

                T[] array = q._array;
                int index = q._head + offset;
                if ((uint)index < (uint)array.Length)
                {
                    _currentElement = array[index];
                }
                else
                {
                    // The index has wrapped around the end of the array. Shift the index and then
                    // get the current element. It is tempting to dedup this dereferencing with that
                    // in the if block above, but the if block above avoids a bounds check for the
                    // accesses that are in that portion, whereas these still incur it.
                    index -= array.Length;
                    _currentElement = array[index];
                }

                return true;
            }

            _i = -2;
            _currentElement = default;
            return false;
        }

        public T Current => _currentElement!;

        object? IEnumerator.Current => Current;

        void IEnumerator.Reset()
        {
            if (_version != _queue._version)
            {
                throw new InvalidOperationException($"Queue {this} was modified during enumeration.");
            }

            _i = -1;
            _currentElement = default;
        }
    }
}