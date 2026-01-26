// Copyright (c) 2026, RickyYC and Contributors. All rights reserved. 
// Distributed under the MIT Software License, Version 1.0.

using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace Faster.Collections.Pooled;

[DebuggerTypeProxy(typeof(IDictionaryDebugView<,>))]
[DebuggerDisplay("Count = {Count}")]
// [Serializable]
public partial class PooledDictionary<TKey, TValue> : FasterDictionary<TKey, TValue>, IDisposable where TKey : notnull
{
    public PooledDictionary() : base(2, 0.8) { }
    public PooledDictionary(int length) : base(length, 0.8) { }
    public PooledDictionary(int length, double loadfactor) : base(length, loadfactor) { }
    
    public PooledDictionary(ReadOnlySpan<KeyValuePair<TKey, TValue>> source) : base(source) { }
    public PooledDictionary(Span<KeyValuePair<TKey, TValue>> source) : base(source) { }
    public PooledDictionary(ReadOnlyMemory<KeyValuePair<TKey, TValue>> source) : base(source) { }
    public PooledDictionary(Memory<KeyValuePair<TKey, TValue>> source) : base(source) { }
    public PooledDictionary(IEnumerable<KeyValuePair<TKey, TValue>> source) : base(source) {}
    public PooledDictionary(KeyValuePair<TKey, TValue>[] source) : base(source) { }
    public PooledDictionary(Dictionary<TKey, TValue> source) : base(source) { }
    public PooledDictionary(IDictionary<TKey, TValue> source) : base(source) { }
    public PooledDictionary(IReadOnlyDictionary<TKey, TValue> source) : base(source) { }

    // Tuple-based constructors
    public PooledDictionary(IEnumerable<(TKey Key, TValue Value)> source) : base(source) { }
    public PooledDictionary(ReadOnlySpan<(TKey Key, TValue Value)> source) : base(source) { }
    public PooledDictionary(Span<(TKey Key, TValue Value)> source) : base(source) { }
    public PooledDictionary(ReadOnlyMemory<(TKey Key, TValue Value)> source) : base(source) { }
    public PooledDictionary(Memory<(TKey Key, TValue Value)> source) : base(source) { }
    public PooledDictionary((TKey Key, TValue Value)[] source) : base(source) { }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override Bucket[] AllocBuckets(int size)
    {
        return SingleThreadedArrayPool<Bucket>.Shared.Rent(size);
    }

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override Entry[] AllocEntries(int size)
    {
        return SingleThreadedArrayPool<Entry>.Shared.Rent(size);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override void FreeBuckets(Bucket[]? buckets)
    {
        if (buckets != null)
            SingleThreadedArrayPool<Bucket>.Shared.Return(buckets);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override void FreeEntries(Entry[]? entries)
    {
        if (entries != null)
            SingleThreadedArrayPool<Entry>.Shared.Return(entries, RuntimeHelpers.IsReferenceOrContainsReferences<Entry>());
    }
}
