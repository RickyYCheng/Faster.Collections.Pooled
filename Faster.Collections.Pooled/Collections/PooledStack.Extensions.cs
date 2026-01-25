// Copyright (c) 2026, RickyYC and Contributors. All rights reserved.
// Distributed under the MIT Software License, Version 1.0.

namespace Faster.Collections.Pooled;

public partial class PooledStack<T>
{
    public PooledStack(ReadOnlySpan<T> source) : this(source.Length)
    {
        source.CopyTo(_array);
        _size = source.Length;
    }
}
