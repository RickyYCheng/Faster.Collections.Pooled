// Copyright (c) 2026, RickyYC and Contributors. All rights reserved.
// Distributed under the MIT Software License, Version 1.0.

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Faster.Collections.Pooled;

public partial class PooledQueue<T>
{
    public PooledQueue(ReadOnlySpan<T> source) : this(source.Length)
    {
        source.CopyTo(_array);
        _size = source.Length;
        if (_size != _array.Length) _tail = _size;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Add(T item) =>
        Enqueue(item);
}

public partial class PooledQueue<T>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void MovePrev(ref int index)
    {
        int tmp = index - 1;
        if (tmp < 0)
        {
            tmp = _array.Length - 1;
        }
        index = tmp;
    }

    /// <summary>
    /// Adds an item to the head (front) of the deque.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void EnqueueHead(T item)
    {
        if (_size == _array.Length)
        {
            Grow(_size + 1);
        }

        MovePrev(ref _head);
        _array[_head] = item;
        _size++;
        _version++;
    }

    /// <summary>
    /// Adds an item to the tail (back) of the deque.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void EnqueueTail(T item) =>
        Enqueue(item);

    /// <summary>
    /// Removes and returns the item at the head (front) of the deque.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T DequeueHead() =>
        Dequeue();

    /// <summary>
    /// Removes and returns the item at the tail (back) of the deque.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T DequeueTail()
    {
        if (_size == 0)
        {
            ThrowForEmptyQueue();
        }

        MovePrev(ref _tail);
        T removed = _array[_tail];
        if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
        {
            _array[_tail] = default!;
        }
        _size--;
        _version++;
        return removed;
    }

    /// <summary>
    /// Returns a reference to the item at the head (front) of the deque.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ref T Head()
    {
        if (_size == 0)
        {
            ThrowForEmptyQueue();
        }

        return ref _array[_head];
    }

    /// <summary>
    /// Returns a reference to the item at the tail (back) of the deque.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ref T Tail()
    {
        if (_size == 0)
        {
            ThrowForEmptyQueue();
        }

        int tailIndex = _tail - 1;
        if (tailIndex < 0)
        {
            tailIndex = _array.Length - 1;
        }
        return ref _array[tailIndex];
    }

    /// <summary>
    /// Tries to remove and return the item at the head (front) of the deque.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryDequeueHead([MaybeNullWhen(false)] out T result) =>
        TryDequeue(out result);

    /// <summary>
    /// Tries to remove and return the item at the tail (back) of the deque.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryDequeueTail([MaybeNullWhen(false)] out T result)
    {
        if (_size == 0)
        {
            result = default;
            return false;
        }

        MovePrev(ref _tail);
        result = _array[_tail];
        if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
        {
            _array[_tail] = default!;
        }
        _size--;
        _version++;
        return true;
    }

    /// <summary>
    /// Tries to return a reference to the item at the head (front) of the deque.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryHead([MaybeNullWhen(false)] out T result)
    {
        if (_size == 0)
        {
            result = default;
            return false;
        }

        result = _array[_head];
        return true;
    }

    /// <summary>
    /// Tries to return a reference to the item at the tail (back) of the deque.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryTail([MaybeNullWhen(false)] out T result)
    {
        if (_size == 0)
        {
            result = default;
            return false;
        }

        int tailIndex = _tail - 1;
        if (tailIndex < 0)
        {
            tailIndex = _array.Length - 1;
        }
        result = _array[tailIndex];
        return true;
    }
}

public static partial class PooledCollectionsExtensions
{
    public static PooledQueue<T> ToPooledQueue<T>(this ReadOnlySpan<T> source) => 
        new PooledQueue<T>(source);
    public static PooledQueue<T> ToPooledQueue<T>(this Span<T> source) => 
        new PooledQueue<T>(source);
    public static PooledQueue<T> ToPooledQueue<T>(this ReadOnlyMemory<T> source) => 
        new PooledQueue<T>(source.Span);
    public static PooledQueue<T> ToPooledQueue<T>(this Memory<T> source) => 
        new PooledQueue<T>(source.Span);
    public static PooledQueue<T> ToPooledQueue<T>(this IEnumerable<T> source) => 
        new PooledQueue<T>(source);
    public static PooledQueue<T> ToPooledQueue<T>(this T[] source) => 
        new PooledQueue<T>(source);
}
