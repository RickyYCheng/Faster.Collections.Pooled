// Copyright (c) 2026, RickyYC and Contributors. All rights reserved.
// Distributed under the MIT Software License, Version 1.0.

using System.Runtime.CompilerServices;

namespace Faster.Collections.Pooled;

public partial class PooledList<T>
{
    public PooledList(ReadOnlySpan<T> span)
        : this(span.Length)
    {
        span.CopyTo(_items);
        _size = span.Length;
    }

    internal Span<T> Span
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _items.AsSpan(0, _size);
    }
    public Span<T> this[Range range]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Span[range];
    }
}

public static partial class PooledCollectionsExtensions
{
    public static void AddRange<T>(this PooledList<T> list, ReadOnlySpan<T> source)
    {
        ArgumentNullException.ThrowIfNull(list);

        if (!source.IsEmpty)
        {
            if (list._items.Length - list._size < source.Length)
            {
                list.Grow(checked(list._size + source.Length));
            }

            source.CopyTo(list._items.AsSpan(list._size));
            list._size += source.Length;
            list._version++;
        }
    }
    public static void CopyTo<T>(this PooledList<T> list, Span<T> destination)
    {
        ArgumentNullException.ThrowIfNull(list);

        new ReadOnlySpan<T>(list._items, 0, list._size).CopyTo(destination);
    }
    public static void InsertRange<T>(this PooledList<T> list, int index, ReadOnlySpan<T> source)
    {
        ArgumentNullException.ThrowIfNull(list);

        if ((uint)index > (uint)list._size)
        {
            throw new IndexOutOfRangeException();
        }

        if (!source.IsEmpty)
        {
            if (list._items.Length - list._size < source.Length)
            {
                list.GrowForInsertion(index, source.Length);
            }
            else if (index < list._size)
            {
                // If the index at which to insert is less than the number of items in the list,
                // shift all items past that location in the list down to the end, making room
                // to copy in the new data.
                Array.Copy(list._items, index, list._items, index + source.Length, list._size - index);
            }

            // Copy the source span into the list.
            // Note that this does not handle the unsafe case of trying to insert a CollectionsMarshal.AsSpan(list)
            // or some slice thereof back into the list itself; such an operation has undefined behavior.
            source.CopyTo(list._items.AsSpan(index));
            list._size += source.Length;
            list._version++;
        }
    }
    public static Span<T> AsSpan<T>(this PooledList<T> list)
    {
        ArgumentNullException.ThrowIfNull(list);

        return new Span<T>(list._items, 0, list._size);
    }
}

public static partial class PooledCollectionsExtensions
{
    public static PooledList<T> ToPooledList<T>(this IEnumerable<T> source) =>
        new PooledList<T>(source);
    public static PooledList<T> ToPooledList<T>(this ReadOnlySpan<T> source) => 
        new PooledList<T>(source);
    public static PooledList<T> ToPooledList<T>(this Span<T> source) =>
        new PooledList<T>(source);
    public static PooledList<T> ToPooledList<T>(this ReadOnlyMemory<T> source) => 
        new PooledList<T>(source.Span);
    public static PooledList<T> ToPooledList<T>(this Memory<T> source) => 
        new PooledList<T>(source.Span);
    public static PooledList<T> ToPooledList<T>(this T[] source) =>
        new PooledList<T>(source);
}
