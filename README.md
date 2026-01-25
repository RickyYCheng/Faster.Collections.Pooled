# Faster.Collections.Pooled

A high-performance collection library for .NET 10+ that provides pooled alternatives to standard collections, reducing GC pressure and improving throughput.

## Features

- **Array Pooling** - All collections leverage shared array pools to minimize allocations
- **High-Performance Hashing** - Includes optimized hash algorithms (FastHash, WyHash, xxHash3, CRC32) powered by [`Faster.Map`](https://github.com/Wsm2110/Faster.Map).  
- **Drop-in Replacements** - Compatible APIs for Dictionary, List, Queue, Stack, and PriorityQueue.  
- **Comprehensive Testing** - Includes comparison tests against BCL collections.  
- **Benchmarking** - Includes a dedicated benchmarking project for performance evaluation.  

## API Compatibility

| Collection | API Source |
|-----------|------------|
| `PooledList<T>` | `List<T>` |
| `PooledQueue<T>` | `Queue<T>` |
| `PooledStack<T>` | `Stack<T>` |
| `PooledPriorityQueue<TElement, TPriority>` | `PriorityQueue<TElement, TPriority>` |
| `PooledDictionary<TKey, TValue>` | `FasterMap.BlitzMap<TKey, TValue>` + Interfaces* of `Dictionary<TKey, TValue>` |

> [!IMPORTANT]  
> \* `PooledDictionary` implements standard interfaces of `Dictionary<TKey, TValue>` except `ISerializable` and `IDeserializationCallback`

## Installation

### NuGet Package

```bash
dotnet add package Faster.Collections.Pooled
```

### Build from Source

```bash
dotnet fsi build.fsx build
```

## Performance

> [!NOTE]  
> Sometimes, benchmark-dotnet or GC will allocate small amount of memory.  
> For exmaple, the benchmark below shows `System.Collections.Generic.PriorityQueue<TElement, TPriority>.Dequeue` allocates `0 B`, but it may allocate a few bytes in some runs.

| Method                     | N      | Mean           | Error        | StdDev       | Median         | Ratio | RatioSD | Allocated | Alloc Ratio |
|--------------------------- |------- |---------------:|-------------:|-------------:|---------------:|------:|--------:|----------:|------------:|
| **OriginPriorityQueueDequeue** | **100**    |     **2,741.8 ns** |    **168.45 ns** |    **472.36 ns** |     **2,700.0 ns** |  **1.03** |    **0.24** |     **336 B** |        **1.00** |
| PooledPriorityQueueDequeue | 100    |     2,847.4 ns |    158.32 ns |    454.26 ns |     2,800.0 ns |  1.07 |    0.24 |         - |        0.00 |
|                            |        |                |              |              |                |       |         |           |             |
| **OriginPriorityQueueDequeue** | **1000**   |    **41,531.0 ns** |    **826.78 ns** |  **1,211.88 ns** |    **41,400.0 ns** |  **1.00** |    **0.04** |         **-** |          **NA** |
| PooledPriorityQueueDequeue | 1000   |    42,242.9 ns |    783.69 ns |    694.72 ns |    42,250.0 ns |  1.02 |    0.03 |         - |          NA | 

### PooledList vs List

#### Add
![List_Add](docs/charts/List_Add.png)
> Speed up 85%, no allocations.

#### Remove
![List_Remove](docs/charts/List_Remove.png)
> Almost same speed, no allocations.

### PooledDictionary vs Dictionary
![Dictionary_Add](docs/charts/Dictionary_Add.png)
> Speed up 298%, no allocations.

![Dictionary_Remove](docs/charts/Dictionary_Remove.png)
> Speed up 58%, no allocations.

### PooledQueue vs Queue

#### Enqueue
![Queue_Enqueue](docs/charts/Queue_Enqueue.png)
> Speed up 65%, no allocations.

#### Dequeue
![Queue_Dequeue](docs/charts/Queue_Dequeue.png)
> Almost same speed, no allocations.

### PooledStack vs Stack

#### Push
![Stack_Push](docs/charts/Stack_Push.png)
> Speed up 86%, no allocations.

#### Pop
![Stack_Pop](docs/charts/Stack_Pop.png)
> Almost same speed, no allocations.

### PooledPriorityQueue vs PriorityQueue

#### Enqueue
![PriorityQueue_Enqueue](docs/charts/PriorityQueue_Enqueue.png)
> Speed up 71%, no allocations.

#### Dequeue
![PriorityQueue_Dequeue](docs/charts/PriorityQueue_Dequeue.png)
> Almost same speed, no allocations.

## Roadmap
- [x] Primitive pooled collections (`List`, `Dictionary`, `Queue`, `Stack`, `PriorityQueue`)
- [ ] Double ended queue (based on PooledQueue)
- [ ] Pooled Collections Extensions
- [ ] Span support for boost performance
- [ ] `PooledHashset` (based on `Faster.Map.BlitzSet`)
- [ ] `ISerializable` and `IDeserializationCallback` support for `PooledDictionary`

## Acknowledgments

> [!IMPORTANT]  
> Very special thanks to the following projects:  
> - [Faster.Map](https://github.com/Wsm2110/Faster.Map) - High-performance hashing algorithms  
> - [Collections.Pooled](https://github.com/jtmueller/Collections.Pooled) - Pooled collection implementations  

## License

[MIT License](LICENSE)
