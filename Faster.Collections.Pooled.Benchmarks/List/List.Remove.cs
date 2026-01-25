// Copyright (c) 2026, RickyYC and Contributors. All rights reserved.
// Distributed under the MIT Software License, Version 1.0.

using BenchmarkDotNet.Attributes;
using Faster.Collections.Pooled;

[Config(typeof(BenchmarkConfig))]
public class List_Remove
{
    [Benchmark(Baseline = true)]
    public void OriginListRemove()
    {
        for (int i = 0; i < N; i++)
        {
            origin_list?.Remove(i);
        }
    }

    [Benchmark]
    public void PooledListRemove()
    {
        for (int i = 0; i < N; i++)
        {
            pooled_list?.Remove(i);
        }
    }

    [Params(10, 100, 1000, 10_000, 100_000)]
    public int N;

    List<int>? origin_list;
    PooledList<int>? pooled_list;

    [IterationSetup]
    public void IterationSetup()
    {
        origin_list = new();
        pooled_list = new();

        for (int i = 0; i < N; i++)
        {
            origin_list.Add(i);
            pooled_list.Add(i);
        }
    }

    [IterationCleanup]
    public void IterationCleanup()
    {
        pooled_list?.Dispose();

        origin_list = null;
        pooled_list = null;
    }
}
