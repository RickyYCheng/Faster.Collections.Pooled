// Copyright (c) 2026, RickyYC and Contributors. All rights reserved.
// Distributed under the MIT Software License, Version 1.0.

using System.Runtime.CompilerServices;

namespace Faster.Collections.Pooled;

public partial class PooledStack<T>
{
    public PooledStack(ReadOnlySpan<T> source) : this(source.Length)
    {
        source.CopyTo(_array);
        _size = source.Length;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Add(T item) =>
        Push(item);
}

public static partial class PooledCollectionsExtensions
{
    public static PooledStack<T> ToPooledStack<T>(this ReadOnlySpan<T> source) => 
        new PooledStack<T>(source);
    public static PooledStack<T> ToPooledStack<T>(this Span<T> source) => 
        new PooledStack<T>(source);
    public static PooledStack<T> ToPooledStack<T>(this ReadOnlyMemory<T> source) => 
        new PooledStack<T>(source.Span);
    public static PooledStack<T> ToPooledStack<T>(this Memory<T> source) => 
        new PooledStack<T>(source.Span);
    public static PooledStack<T> ToPooledStack<T>(this IEnumerable<T> source) => 
        new PooledStack<T>(source);
    public static PooledStack<T> ToPooledStack<T>(this T[] source) => 
        new PooledStack<T>(source);
}
