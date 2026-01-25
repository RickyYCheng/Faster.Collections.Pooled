// Copyright (c) 2026, RickyYC and Contributors. All rights reserved.
// Distributed under the MIT Software License, Version 1.0.

namespace Faster.Collections.Pooled.Tests.Queue;

public class PooledQueueExtensionsTests
{
    [Fact]
    public void Constructor_FromReadOnlySpan_CreatesQueueWithCorrectItems()
    {
        // Arrange
        var source = new ReadOnlySpan<int>([1, 2, 3, 4, 5]);

        // Act
        var queue = new PooledQueue<int>(source);

        // Assert
        Assert.Equal(5, queue.Count);
        Assert.Equal(1, queue.Dequeue());
        Assert.Equal(2, queue.Dequeue());
        Assert.Equal(3, queue.Dequeue());
        Assert.Equal(4, queue.Dequeue());
        Assert.Equal(5, queue.Dequeue());
        Assert.Empty(queue);
    }

    [Fact]
    public void Constructor_FromEmptySpan_CreatesEmptyQueue()
    {
        // Arrange
        var source = ReadOnlySpan<int>.Empty;

        // Act
        var queue = new PooledQueue<int>(source);

        // Assert
        Assert.Empty(queue);
    }

    [Fact]
    public void Constructor_FromSingleElementSpan_WorksCorrectly()
    {
        // Arrange
        var source = new ReadOnlySpan<int>([42]);

        // Act
        var queue = new PooledQueue<int>(source);

        // Assert
        Assert.Single(queue);
        Assert.Equal(42, queue.Dequeue());
    }

    [Fact]
    public void Constructor_WithReferenceTypes_WorksCorrectly()
    {
        // Arrange
        var source = new ReadOnlySpan<string>(["a", "b", "c"]);

        // Act
        var queue = new PooledQueue<string>(source);

        // Assert
        Assert.Equal(3, queue.Count);
        Assert.Equal("a", queue.Dequeue());
        Assert.Equal("b", queue.Dequeue());
        Assert.Equal("c", queue.Dequeue());
    }

    [Fact]
    public void Constructor_LargeSpan_WorksCorrectly()
    {
        // Arrange
        var data = new int[1000];
        for (int i = 0; i < data.Length; i++)
        {
            data[i] = i + 1;
        }
        var source = new ReadOnlySpan<int>(data);

        // Act
        var queue = new PooledQueue<int>(source);

        // Assert
        Assert.Equal(1000, queue.Count);
        Assert.Equal(1, queue.Dequeue());
        Assert.Equal(2, queue.Dequeue());
        Assert.Equal(3, queue.Dequeue());
    }

    [Fact]
    public void Constructor_FollowedByDequeOperations_WorksCorrectly()
    {
        // Arrange
        var source = new ReadOnlySpan<int>([1, 2, 3]);

        // Act
        var queue = new PooledQueue<int>(source);
        queue.EnqueueHead(0);
        queue.EnqueueTail(4);

        // Assert
        Assert.Equal(5, queue.Count);
        Assert.Equal(0, queue.DequeueHead());
        Assert.Equal(1, queue.DequeueHead());
        Assert.Equal(2, queue.DequeueHead());
        Assert.Equal(3, queue.DequeueHead());
        Assert.Equal(4, queue.DequeueHead());
    }

    [Fact]
    public void Constructor_ThenPeekOperations_WorksCorrectly()
    {
        // Arrange
        var source = new ReadOnlySpan<int>([10, 20, 30]);

        // Act
        var queue = new PooledQueue<int>(source);

        // Assert
        Assert.True(queue.TryHead(out int headValue));
        Assert.Equal(10, headValue);
        Assert.True(queue.TryTail(out int tailValue));
        Assert.Equal(30, tailValue);
        Assert.Equal(3, queue.Count); // Verify no items were removed
    }

    [Fact]
    public void Constructor_WithNullStrings_HandlesCorrectly()
    {
        // Arrange
        var source = new ReadOnlySpan<string?>(["a", null, "b"]);

        // Act
        var queue = new PooledQueue<string?>(source);

        // Assert
        Assert.Equal(3, queue.Count);
        Assert.Equal("a", queue.Dequeue());
        Assert.Null(queue.Dequeue());
        Assert.Equal("b", queue.Dequeue());
    }

    [Fact]
    public void Constructor_FollowedByGrowth_WorksCorrectly()
    {
        // Arrange
        var source = new ReadOnlySpan<int>([1, 2, 3, 4]);

        // Act
        var queue = new PooledQueue<int>(source);
        queue.Enqueue(5); // This should trigger growth

        // Assert
        Assert.Equal(5, queue.Count);
        Assert.Equal(1, queue.Dequeue());
        Assert.Equal(2, queue.Dequeue());
        Assert.Equal(3, queue.Dequeue());
        Assert.Equal(4, queue.Dequeue());
        Assert.Equal(5, queue.Dequeue());
    }

    [Fact]
    public void Constructor_ClearAndReuse_WorksCorrectly()
    {
        // Arrange
        var source = new ReadOnlySpan<int>([1, 2, 3]);
        var queue = new PooledQueue<int>(source);

        // Act
        queue.Clear();
        queue.Enqueue(10);
        queue.Enqueue(20);

        // Assert
        Assert.Equal(2, queue.Count);
        Assert.Equal(10, queue.Dequeue());
        Assert.Equal(20, queue.Dequeue());
    }

    [Fact]
    public void Constructor_CopyToSpan_WorksCorrectly()
    {
        // Arrange
        var source = new ReadOnlySpan<int>([1, 2, 3, 4, 5]);
        var queue = new PooledQueue<int>(source);

        // Act
        var array = new int[5];
        queue.CopyTo(array, 0);

        // Assert
        Assert.Equal([1, 2, 3, 4, 5], array);
    }

    [Fact]
    public void Constructor_WithEmptyArrayAfterOperations_WorksCorrectly()
    {
        // Arrange
        var source = new ReadOnlySpan<int>([1, 2, 3]);
        var queue = new PooledQueue<int>(source);

        // Act - Dequeue all items
        queue.Dequeue();
        queue.Dequeue();
        queue.Dequeue();

        // Assert
        Assert.Empty(queue);
        Assert.False(queue.TryDequeue(out _));
        Assert.False(queue.TryHead(out _));
        Assert.False(queue.TryTail(out _));
    }

    [Fact]
    public void Constructor_ThenDequeueTail_WorksCorrectly()
    {
        // Arrange
        var source = new ReadOnlySpan<int>([1, 2, 3, 4, 5]);

        // Act
        var queue = new PooledQueue<int>(source);
        var tail = queue.DequeueTail();

        // Assert
        Assert.Equal(5, tail);
        Assert.Equal(4, queue.Count);
        Assert.Equal(1, queue.DequeueHead());
    }

    [Fact]
    public void Constructor_TryDequeueTail_WorksCorrectly()
    {
        // Arrange
        var source = new ReadOnlySpan<int>([1, 2, 3]);

        // Act
        var queue = new PooledQueue<int>(source);
        var success = queue.TryDequeueTail(out int result);

        // Assert
        Assert.True(success);
        Assert.Equal(3, result);
        Assert.Equal(2, queue.Count);
    }

    [Fact]
    public void Constructor_MixedHeadTailOperations_WorksCorrectly()
    {
        // Arrange
        var source = new ReadOnlySpan<int>([2, 3, 4]);

        // Act
        var queue = new PooledQueue<int>(source);
        queue.EnqueueHead(1);        // Queue: 1, 2, 3, 4
        queue.EnqueueTail(5);         // Queue: 1, 2, 3, 4, 5

        // Assert
        Assert.Equal(5, queue.Count);
        Assert.Equal(1, queue.DequeueHead());
        Assert.Equal(5, queue.DequeueTail());
        Assert.Equal(2, queue.DequeueHead());
        Assert.Equal(4, queue.DequeueTail());
        Assert.Equal(3, queue.DequeueHead());
    }

    [Fact]
    public void Constructor_AfterWrapping_WorksCorrectly()
    {
        // Arrange
        var source = new ReadOnlySpan<int>([1, 2, 3]);
        var queue = new PooledQueue<int>(source);

        // Act - Create wrapping scenario
        queue.Dequeue();           // Remove 1, queue: 2, 3
        queue.Enqueue(4);          // Add 4, queue: 2, 3, 4
        queue.EnqueueHead(0);      // Add 0 at front, queue: 0, 2, 3, 4

        // Assert
        Assert.Equal(4, queue.Count);
        Assert.Equal(0, queue.DequeueHead());
        Assert.Equal(2, queue.DequeueHead());
        Assert.Equal(3, queue.DequeueHead());
        Assert.Equal(4, queue.DequeueHead());
    }

    [Fact]
    public void Constructor_ToArray_WorksCorrectly()
    {
        // Arrange
        var source = new ReadOnlySpan<int>([1, 2, 3, 4, 5]);

        // Act
        var queue = new PooledQueue<int>(source);
        var array = queue.ToArray();

        // Assert
        Assert.Equal(5, array.Length);
        Assert.Equal([1, 2, 3, 4, 5], array);
    }

    [Fact]
    public void Constructor_WithDuplicates_WorksCorrectly()
    {
        // Arrange
        var source = new ReadOnlySpan<int>([1, 1, 2, 2, 3, 3]);

        // Act
        var queue = new PooledQueue<int>(source);

        // Assert
        Assert.Equal(6, queue.Count);
        Assert.Equal(1, queue.Dequeue());
        Assert.Equal(1, queue.Dequeue());
        Assert.Equal(2, queue.Dequeue());
        Assert.Equal(2, queue.Dequeue());
        Assert.Equal(3, queue.Dequeue());
        Assert.Equal(3, queue.Dequeue());
    }

    [Fact]
    public void Constructor_Enumerator_WorksCorrectly()
    {
        // Arrange
        var source = new ReadOnlySpan<int>([1, 2, 3]);

        // Act
        var queue = new PooledQueue<int>(source);
        var result = queue.ToList();

        // Assert
        Assert.Equal([1, 2, 3], result);
    }

    [Fact]
    public void Constructor_Contains_WorksCorrectly()
    {
        // Arrange
        var source = new ReadOnlySpan<int>([1, 2, 3, 4, 5]);

        // Act
        var queue = new PooledQueue<int>(source);

        // Assert
        Assert.True(queue.Contains(3));
        Assert.False(queue.Contains(10));
    }

    [Fact]
    public void Constructor_WithLargeSpanAndOperations_WorksCorrectly()
    {
        // Arrange
        var data = new int[100];
        for (int i = 0; i < data.Length; i++)
        {
            data[i] = i + 1;
        }
        var source = new ReadOnlySpan<int>(data);

        // Act
        var queue = new PooledQueue<int>(source);
        queue.EnqueueHead(0);
        queue.EnqueueTail(101);

        // Assert
        Assert.Equal(102, queue.Count);
        Assert.Equal(0, queue.DequeueHead());
        Assert.Equal(1, queue.DequeueHead());
        Assert.Equal(2, queue.DequeueHead());
        Assert.Equal(101, queue.DequeueTail());
    }

    [Fact]
    public void Constructor_ThenTrimExcess_WorksCorrectly()
    {
        // Arrange
        var source = new ReadOnlySpan<int>([1, 2, 3, 4, 5, 6, 7, 8]);
        var queue = new PooledQueue<int>(source);

        // Act
        for (int i = 0; i < 5; i++)
        {
            queue.Dequeue();
        }
        queue.TrimExcess();

        // Assert
        Assert.Equal(3, queue.Count);
        Assert.Equal(6, queue.Dequeue());
        Assert.Equal(7, queue.Dequeue());
        Assert.Equal(8, queue.Dequeue());
    }
}
