using BenchmarkDotNet.Attributes;
using Faster.Collections.Pooled;

[Config(typeof(BenchmarkConfig))]
public class PriorityQueue_Enqueue
{
    [Benchmark(Baseline = true)]
    public void OriginPriorityQueueEnqueue()
    {
        for (int i = 0; i < N; i++)
        {
            origin_pq?.Enqueue(i, i % factor);
        }
    }

    [Benchmark]
    public void PooledPriorityQueueEnqueue()
    {
        for (int i = 0; i < N; i++)
        {
            pooled_pq?.Enqueue(i, i % factor);
        }
    }

    [Params(10, 100, 1000, 10_000, 100_000)]
    public int N;

    PriorityQueue<int, int>? origin_pq;
    PooledPriorityQueue<int, int>? pooled_pq;
    int factor = 0;

    [IterationSetup]
    public void IterationSetup()
    {
        origin_pq = new();
        pooled_pq = new();
        var rnd = new Random(N);
        factor = rnd.Next(N / 10, N / 5);
    }

    [IterationCleanup]
    public void IterationCleanup()
    {
        pooled_pq?.Dispose();

        origin_pq = null;
        pooled_pq = null;
    }
}
