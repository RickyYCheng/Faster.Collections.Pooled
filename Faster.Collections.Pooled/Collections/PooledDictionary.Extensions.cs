// Copyright (c) 2026, RickyYC and Contributors. All rights reserved. 
// Distributed under the MIT Software License, Version 1.0.

namespace Faster.Collections.Pooled;

public static partial class PooledCollectionsExtensions
{
    public static PooledDictionary<TKey, TValue> ToPooledDictionary<TKey, TValue>(this Dictionary<TKey, TValue> source)
        where TKey : notnull =>
        new PooledDictionary<TKey, TValue>(source);
    public static PooledDictionary<TKey, TValue> ToPooledDictionary<TKey, TValue>(this IDictionary<TKey, TValue> source)
        where TKey : notnull =>
        new PooledDictionary<TKey, TValue>(source);
    public static PooledDictionary<TKey, TValue> ToPooledDictionary<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> source)
        where TKey : notnull =>
        new PooledDictionary<TKey, TValue>(source);
    public static PooledDictionary<TKey, TValue> ToPooledDictionary<TKey, TValue>(this ReadOnlySpan<KeyValuePair<TKey, TValue>> source)
        where TKey : notnull =>
        new PooledDictionary<TKey, TValue>(source);
    public static PooledDictionary<TKey, TValue> ToPooledDictionary<TKey, TValue>(this Span<KeyValuePair<TKey, TValue>> source)
        where TKey : notnull =>
        new PooledDictionary<TKey, TValue>(source);
    public static PooledDictionary<TKey, TValue> ToPooledDictionary<TKey, TValue>(this ReadOnlyMemory<KeyValuePair<TKey, TValue>> source)
        where TKey : notnull =>
        new PooledDictionary<TKey, TValue>(source);
    public static PooledDictionary<TKey, TValue> ToPooledDictionary<TKey, TValue>(this Memory<KeyValuePair<TKey, TValue>> source)
        where TKey : notnull =>
        new PooledDictionary<TKey, TValue>(source);
    public static PooledDictionary<TKey, TValue> ToPooledDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> source)
        where TKey : notnull =>
        new PooledDictionary<TKey, TValue>(source);
    public static PooledDictionary<TKey, TValue> ToPooledDictionary<TKey, TValue>(this KeyValuePair<TKey, TValue>[] source)
        where TKey : notnull =>
        new PooledDictionary<TKey, TValue>(source);

    // Tuple-based extension methods
    public static PooledDictionary<TKey, TValue> ToPooledDictionary<TKey, TValue>(this IEnumerable<(TKey Key, TValue Value)> source)
        where TKey : notnull =>
        new PooledDictionary<TKey, TValue>(source);
    public static PooledDictionary<TKey, TValue> ToPooledDictionary<TKey, TValue>(this ReadOnlySpan<(TKey Key, TValue Value)> source)
        where TKey : notnull =>
        new PooledDictionary<TKey, TValue>(source);
    public static PooledDictionary<TKey, TValue> ToPooledDictionary<TKey, TValue>(this Span<(TKey Key, TValue Value)> source)
        where TKey : notnull =>
        new PooledDictionary<TKey, TValue>(source);
    public static PooledDictionary<TKey, TValue> ToPooledDictionary<TKey, TValue>(this ReadOnlyMemory<(TKey Key, TValue Value)> source)
        where TKey : notnull =>
        new PooledDictionary<TKey, TValue>(source);
    public static PooledDictionary<TKey, TValue> ToPooledDictionary<TKey, TValue>(this Memory<(TKey Key, TValue Value)> source)
        where TKey : notnull =>
        new PooledDictionary<TKey, TValue>(source);
    public static PooledDictionary<TKey, TValue> ToPooledDictionary<TKey, TValue>(this (TKey Key, TValue Value)[] source)
        where TKey : notnull =>
        new PooledDictionary<TKey, TValue>(source);
}