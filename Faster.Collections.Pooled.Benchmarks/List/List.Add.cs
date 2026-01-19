using BenchmarkDotNet.Attributes;
using Faster.Collections.Pooled;

[Config(typeof(BenchmarkConfig))]
public class List_Add
{
    [Benchmark(Baseline = true)]
    public void OriginListAdd()
    {
        for (int i = 0; i < N; i++)
        {
            origin_list?.Add(i);
        }
    }

    [Benchmark]
    public void PooledListAdd()
    {
        for (int i = 0; i < N; i++)
        {
            pooled_list?.Add(i);
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
    }

    [IterationCleanup]
    public void IterationCleanup()
    {
        pooled_list?.Dispose();

        origin_list = null;
        pooled_list = null;
    }
}
