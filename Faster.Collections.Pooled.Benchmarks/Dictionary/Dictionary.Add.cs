
using BenchmarkDotNet.Attributes;
using Faster.Collections.Pooled;

[Config(typeof(BenchmarkConfig))]
public class Dictionary_Add
{
    [Benchmark(Baseline = true)]
    public void OriginDictAdd()
    {
        for (int i = 0; i < N; i++)
        {
            origin_dict?.Add(i, i);
        }
    }

    [Benchmark]
    public void FasterDictAdd()
    {
        for (int i = 0; i < N; i++)
        {
            faster_dict?.Insert(i, i);
        }
    }

    [Benchmark]
    public void PooledDictAdd()
    {
        for (int i = 0; i < N; i++)
        {
            pooled_dict?.Insert(i, i);
        }
    }

    [Params(10, 100, 1000, 10_000, 100_000)]
    public int N;

    Dictionary<int, int>? origin_dict;
    FasterDictionary<int, int>? faster_dict;
    PooledDictionary<int, int>? pooled_dict;

    [IterationSetup]
    public void IterationSetup()
    {
        origin_dict = new();
        faster_dict = new();
        pooled_dict = new();
    }

    [IterationCleanup]
    public void IterationCleanup()
    {
        pooled_dict?.Dispose();

        origin_dict = null;
        faster_dict = null;
        pooled_dict = null;
    }
}
