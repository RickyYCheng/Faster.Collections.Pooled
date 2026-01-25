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
