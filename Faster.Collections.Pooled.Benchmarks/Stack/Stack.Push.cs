using BenchmarkDotNet.Attributes;
using Faster.Collections.Pooled;

[Config(typeof(BenchmarkConfig))]
public class Stack_Push
{
    [Benchmark(Baseline = true)]
    public void OriginStackPush()
    {
        for (int i = 0; i < N; i++)
        {
            origin_stack?.Push(i);
        }
    }

    [Benchmark]
    public void PooledStackPush()
    {
        for (int i = 0; i < N; i++)
        {
            pooled_stack?.Push(i);
        }
    }

    [Params(10, 100, 1000, 10_000, 100_000)]
    public int N;

    Stack<int>? origin_stack;
    PooledStack<int>? pooled_stack;

    [IterationSetup]
    public void IterationSetup()
    {
        origin_stack = new();
        pooled_stack = new();
    }

    [IterationCleanup]
    public void IterationCleanup()
    {
        pooled_stack?.Dispose();

        origin_stack = null;
        pooled_stack = null;
    }
}
