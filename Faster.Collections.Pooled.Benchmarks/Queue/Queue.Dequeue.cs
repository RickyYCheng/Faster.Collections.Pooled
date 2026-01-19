using BenchmarkDotNet.Attributes;
using Faster.Collections.Pooled;

[Config(typeof(BenchmarkConfig))]
public class Queue_Dequeue
{
    [Benchmark(Baseline = true)]
    public void OriginQueueDequeue()
    {
        for (int i = 0; i < N; i++)
        {
            origin_queue?.Dequeue();
        }
    }

    [Benchmark]
    public void PooledQueueDequeue()
    {
        for (int i = 0; i < N; i++)
        {
            pooled_queue?.Dequeue();
        }
    }

    [Params(10, 100, 1000, 10_000, 100_000)]
    public int N;

    Queue<int>? origin_queue;
    PooledQueue<int>? pooled_queue;

    [IterationSetup]
    public void IterationSetup()
    {
        origin_queue = new();
        pooled_queue = new();

        for (int i = 0; i < N; i++)
        {
            origin_queue.Enqueue(i);
            pooled_queue.Enqueue(i);
        }
    }

    [IterationCleanup]
    public void IterationCleanup()
    {
        pooled_queue?.Dispose();

        origin_queue = null;
        pooled_queue = null;
    }
}
