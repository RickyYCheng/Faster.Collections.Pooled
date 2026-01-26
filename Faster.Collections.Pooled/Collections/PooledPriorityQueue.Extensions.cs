// Copyright (c) 2026, RickyYC and Contributors. All rights reserved.
// Distributed under the MIT Software License, Version 1.0.

namespace Faster.Collections.Pooled;

public partial class PooledPriorityQueue<TElement, TPriority>
{
    public PooledPriorityQueue(ReadOnlySpan<(TElement Element, TPriority Priority)> items, IComparer<TPriority>? comparer)
    {
        _nodes = Alloc(items.Length);
        items.CopyTo(_nodes);
        _size = items.Length;

        _comparer = InitializeComparer(comparer);

        if (_size > 1)
        {
            Heapify();
        }
    }
    public PooledPriorityQueue(ReadOnlySpan<(TElement Element, TPriority Priority)> items)
        : this(items, null)
    {
    }
}

public static partial class PooledCollectionsExtensions
{
    public static PooledPriorityQueue<TElement, TPriority> ToPooledPriorityQueue<TElement, TPriority>(this ReadOnlySpan<(TElement Element, TPriority Priority)> source, IComparer<TPriority>? comparer = null) => 
        new PooledPriorityQueue<TElement, TPriority>(source, comparer);
    public static PooledPriorityQueue<TElement, TPriority> ToPooledPriorityQueue<TElement, TPriority>(this Span<(TElement Element, TPriority Priority)> source, IComparer<TPriority>? comparer = null) => 
        new PooledPriorityQueue<TElement, TPriority>(source, comparer);
    public static PooledPriorityQueue<TElement, TPriority> ToPooledPriorityQueue<TElement, TPriority>(this ReadOnlyMemory<(TElement Element, TPriority Priority)> source, IComparer<TPriority>? comparer = null) => 
        new PooledPriorityQueue<TElement, TPriority>(source.Span, comparer);
    public static PooledPriorityQueue<TElement, TPriority> ToPooledPriorityQueue<TElement, TPriority>(this Memory<(TElement Element, TPriority Priority)> source, IComparer<TPriority>? comparer = null) => 
        new PooledPriorityQueue<TElement, TPriority>(source.Span, comparer);
    public static PooledPriorityQueue<TElement, TPriority> ToPooledPriorityQueue<TElement, TPriority>(this IEnumerable<(TElement Element, TPriority Priority)> source, IComparer<TPriority>? comparer = null) =>
        new PooledPriorityQueue<TElement, TPriority>(source, comparer);
    public static PooledPriorityQueue<TElement, TPriority> ToPooledPriorityQueue<TElement, TPriority>(this (TElement Element, TPriority Priority)[] source, IComparer<TPriority>? comparer = null) => 
        new PooledPriorityQueue<TElement, TPriority>(source, comparer);
}
