// Copyright (c) 2026, RickyYC and Contributors. All rights reserved. 
// Distributed under the MIT Software License, Version 1.0.

using System.Runtime.CompilerServices;

namespace Faster.Collections.Pooled;

public partial class FasterDictionary<TKey, TValue>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Add((TKey Key, TValue Value) item)
    {
        Add(item.Key, item.Value);
    }
}
