// Copyright (c) 2026, RickyYC and Contributors. All rights reserved.
// Distributed under the MIT Software License, Version 1.0.

namespace Faster.Collections.Pooled.Tests.Queue;

public class DequeTests
{
    [Fact]
    public void EnqueueHead_AddsToFront()
    {
        // Arrange
        var queue = new PooledQueue<int>();
        queue.Enqueue(1);
        queue.Enqueue(2);

        // Act
        queue.EnqueueHead(0);

        // Assert
        Assert.Equal(3, queue.Count);
        Assert.Equal(0, queue.Dequeue());
        Assert.Equal(1, queue.Dequeue());
    }

    [Fact]
    public void EnqueueTail_AddsToBack()
    {
        // Arrange
        var queue = new PooledQueue<int>();

        // Act
        queue.EnqueueTail(1);
        queue.EnqueueTail(2);
        queue.EnqueueTail(3);

        // Assert
        Assert.Equal(3, queue.Count);
        Assert.Equal(1, queue.Dequeue());
        Assert.Equal(2, queue.Dequeue());
        Assert.Equal(3, queue.Dequeue());
    }

    [Fact]
    public void DequeueHead_RemovesFromFront()
    {
        // Arrange
        var queue = new PooledQueue<int>();
        queue.EnqueueHead(3);
        queue.EnqueueHead(2);
        queue.EnqueueHead(1);

        // Act
        var result = queue.DequeueHead();

        // Assert
        Assert.Equal(1, result);
        Assert.Equal(2, queue.Count);
        Assert.Equal(2, queue.DequeueHead());
    }

    [Fact]
    public void DequeueTail_RemovesFromBack()
    {
        // Arrange
        var queue = new PooledQueue<int>();
        queue.Enqueue(1);
        queue.Enqueue(2);
        queue.Enqueue(3);

        // Act
        var result = queue.DequeueTail();

        // Assert
        Assert.Equal(3, result);
        Assert.Equal(2, queue.Count);
        Assert.Equal(2, queue.DequeueTail());
    }

    [Fact]
    public void DequeueHead_EmptyQueue_ThrowsException()
    {
        // Arrange
        var queue = new PooledQueue<int>();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => queue.DequeueHead());
    }

    [Fact]
    public void DequeueTail_EmptyQueue_ThrowsException()
    {
        // Arrange
        var queue = new PooledQueue<int>();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => queue.DequeueTail());
    }

    [Fact]
    public void Head_ReturnsReferenceToFrontElement()
    {
        // Arrange
        var queue = new PooledQueue<int>();
        queue.Enqueue(10);
        queue.Enqueue(20);
        queue.Enqueue(30);

        // Act
        ref var head = ref queue.Head();
        head = 100;

        // Assert
        Assert.Equal(100, queue.Dequeue());
        Assert.Equal(20, queue.Dequeue());
    }

    [Fact]
    public void Tail_ReturnsReferenceToBackElement()
    {
        // Arrange
        var queue = new PooledQueue<int>();
        queue.Enqueue(10);
        queue.Enqueue(20);
        queue.Enqueue(30);

        // Act
        ref var tail = ref queue.Tail();
        tail = 300;

        // Assert
        Assert.Equal(300, queue.DequeueTail());
        Assert.Equal(20, queue.DequeueTail());
    }

    [Fact]
    public void Head_EmptyQueue_ThrowsException()
    {
        // Arrange
        var queue = new PooledQueue<int>();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => queue.Head());
    }

    [Fact]
    public void Tail_EmptyQueue_ThrowsException()
    {
        // Arrange
        var queue = new PooledQueue<int>();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => queue.Tail());
    }

    [Fact]
    public void TryDequeueHead_EmptyQueue_ReturnsFalse()
    {
        // Arrange
        var queue = new PooledQueue<int>();

        // Act
        var result = queue.TryDequeueHead(out int value);

        // Assert
        Assert.False(result);
        Assert.Equal(0, value);
    }

    [Fact]
    public void TryDequeueTail_EmptyQueue_ReturnsFalse()
    {
        // Arrange
        var queue = new PooledQueue<int>();

        // Act
        var result = queue.TryDequeueTail(out int value);

        // Assert
        Assert.False(result);
        Assert.Equal(0, value);
    }

    [Fact]
    public void TryDequeueHead_WithItems_ReturnsTrueAndValue()
    {
        // Arrange
        var queue = new PooledQueue<int>();
        queue.Enqueue(1);
        queue.Enqueue(2);
        queue.Enqueue(3);

        // Act
        var result = queue.TryDequeueHead(out int value);

        // Assert
        Assert.True(result);
        Assert.Equal(1, value);
        Assert.Equal(2, queue.Count);
    }

    [Fact]
    public void TryDequeueTail_WithItems_ReturnsTrueAndValue()
    {
        // Arrange
        var queue = new PooledQueue<int>();
        queue.Enqueue(1);
        queue.Enqueue(2);
        queue.Enqueue(3);

        // Act
        var result = queue.TryDequeueTail(out int value);

        // Assert
        Assert.True(result);
        Assert.Equal(3, value);
        Assert.Equal(2, queue.Count);
    }

    [Fact]
    public void Deque_MixedOperations_WorksCorrectly()
    {
        // Arrange
        var queue = new PooledQueue<int>();

        // Act
        queue.EnqueueTail(1);
        queue.EnqueueTail(2);
        queue.EnqueueHead(0);
        queue.EnqueueHead(-1);

        // Assert - order should be: -1, 0, 1, 2
        Assert.Equal(4, queue.Count);
        Assert.Equal(-1, queue.DequeueHead());
        Assert.Equal(0, queue.DequeueHead());
        Assert.Equal(2, queue.DequeueTail());
        Assert.Equal(1, queue.DequeueTail());
        Assert.Empty(queue);
    }

    [Fact]
    public void Deque_AfterWrapping_WorksCorrectly()
    {
        // Arrange
        var queue = new PooledQueue<int>(2);
        queue.Enqueue(1);
        queue.Enqueue(2);
        queue.Dequeue();

        // Act
        queue.EnqueueHead(0);
        queue.EnqueueTail(3);

        // Assert
        Assert.Equal(3, queue.Count);
        Assert.Equal(0, queue.DequeueHead());
        Assert.Equal(2, queue.DequeueHead());
        Assert.Equal(3, queue.DequeueHead());
    }

    [Fact]
    public void EnqueueHead_TriggersGrowth_WhenFull()
    {
        // Arrange
        var queue = new PooledQueue<int>(16);
        queue.Enqueue(1);
        queue.Enqueue(2);

        // Act
        queue.EnqueueHead(0);

        // Assert
        Assert.Equal(3, queue.Count);
        Assert.Equal(0, queue.DequeueHead());
        Assert.Equal(1, queue.DequeueHead());
        Assert.Equal(2, queue.DequeueHead());
    }

    [Fact]
    public void HeadAndTail_SameElement_WhenCountIsOne()
    {
        // Arrange
        var queue = new PooledQueue<int>();
        queue.Enqueue(42);

        // Act
        ref var head = ref queue.Head();
        ref var tail = ref queue.Tail();

        // Assert
        Assert.Equal(42, head);
        Assert.Equal(42, tail);
        head = 100;
        Assert.Equal(100, tail);
    }

    [Fact]
    public void TryHead_EmptyQueue_ReturnsFalse()
    {
        // Arrange
        var queue = new PooledQueue<int>();

        // Act
        var result = queue.TryHead(out int value);

        // Assert
        Assert.False(result);
        Assert.Equal(0, value);
    }

    [Fact]
    public void TryTail_EmptyQueue_ReturnsFalse()
    {
        // Arrange
        var queue = new PooledQueue<int>();

        // Act
        var result = queue.TryTail(out int value);

        // Assert
        Assert.False(result);
        Assert.Equal(0, value);
    }

    [Fact]
    public void TryHead_WithItems_ReturnsTrueAndValue()
    {
        // Arrange
        var queue = new PooledQueue<int>();
        queue.Enqueue(10);
        queue.Enqueue(20);
        queue.Enqueue(30);

        // Act
        var result = queue.TryHead(out int value);

        // Assert
        Assert.True(result);
        Assert.Equal(10, value);
        Assert.Equal(3, queue.Count); // Ensure no item was removed
    }

    [Fact]
    public void TryTail_WithItems_ReturnsTrueAndValue()
    {
        // Arrange
        var queue = new PooledQueue<int>();
        queue.Enqueue(10);
        queue.Enqueue(20);
        queue.Enqueue(30);

        // Act
        var result = queue.TryTail(out int value);

        // Assert
        Assert.True(result);
        Assert.Equal(30, value);
        Assert.Equal(3, queue.Count); // Ensure no item was removed
    }

    [Fact]
    public void TryHead_SingleElement_ReturnsTrueAndValue()
    {
        // Arrange
        var queue = new PooledQueue<int>();
        queue.Enqueue(42);

        // Act
        var result = queue.TryHead(out int value);

        // Assert
        Assert.True(result);
        Assert.Equal(42, value);
        Assert.Single(queue);
    }

    [Fact]
    public void TryTail_SingleElement_ReturnsTrueAndValue()
    {
        // Arrange
        var queue = new PooledQueue<int>();
        queue.Enqueue(42);

        // Act
        var result = queue.TryTail(out int value);

        // Assert
        Assert.True(result);
        Assert.Equal(42, value);
        Assert.Single(queue);
    }

    [Fact]
    public void TryHeadAndTryTail_ReturnConsistentValues()
    {
        // Arrange
        var queue = new PooledQueue<int>();
        queue.Enqueue(1);
        queue.Enqueue(2);
        queue.Enqueue(3);

        // Act
        var headSuccess = queue.TryHead(out int headValue);
        var tailSuccess = queue.TryTail(out int tailValue);

        // Assert
        Assert.True(headSuccess);
        Assert.True(tailSuccess);
        Assert.Equal(1, headValue);
        Assert.Equal(3, tailValue);
        Assert.Equal(1, queue.Head()); // Should match TryHead
        Assert.Equal(3, queue.Tail()); // Should match TryTail
    }

    [Fact]
    public void TryHead_AfterDequeue_ReturnsNewHead()
    {
        // Arrange
        var queue = new PooledQueue<int>();
        queue.Enqueue(1);
        queue.Enqueue(2);
        queue.Enqueue(3);
        queue.Dequeue();

        // Act
        var result = queue.TryHead(out int value);

        // Assert
        Assert.True(result);
        Assert.Equal(2, value);
    }

    [Fact]
    public void TryTail_AfterDequeueTail_ReturnsNewTail()
    {
        // Arrange
        var queue = new PooledQueue<int>();
        queue.Enqueue(1);
        queue.Enqueue(2);
        queue.Enqueue(3);
        queue.DequeueTail();

        // Act
        var result = queue.TryTail(out int value);

        // Assert
        Assert.True(result);
        Assert.Equal(2, value);
    }

    [Fact]
    public void TryHead_WithNullableType_ReturnsFalseWhenEmpty()
    {
        // Arrange
        var queue = new PooledQueue<string>();

        // Act
        var result = queue.TryHead(out string? value);

        // Assert
        Assert.False(result);
        Assert.Null(value);
    }

    [Fact]
    public void TryTail_WithNullableType_ReturnsFalseWhenEmpty()
    {
        // Arrange
        var queue = new PooledQueue<string>();

        // Act
        var result = queue.TryTail(out string? value);

        // Assert
        Assert.False(result);
        Assert.Null(value);
    }

    [Fact]
    public void TryHead_AfterWrapping_WorksCorrectly()
    {
        // Arrange
        var queue = new PooledQueue<int>(2);
        queue.Enqueue(1);
        queue.Enqueue(2);
        queue.Dequeue();
        queue.Enqueue(3);

        // Act
        var result = queue.TryHead(out int value);

        // Assert
        Assert.True(result);
        Assert.Equal(2, value);
    }

    [Fact]
    public void TryTail_AfterWrapping_WorksCorrectly()
    {
        // Arrange
        var queue = new PooledQueue<int>(2);
        queue.Enqueue(1);
        queue.Enqueue(2);
        queue.Dequeue();
        queue.Enqueue(3);

        // Act
        var result = queue.TryTail(out int value);

        // Assert
        Assert.True(result);
        Assert.Equal(3, value);
    }

    [Fact]
    public void EnqueueHead_TriggersGrowth_MaintainsCorrectOrder()
    {
        // Arrange
        var queue = new PooledQueue<int>(16);
        for (int i = 0; i < 16; i++)
        {
            queue.EnqueueTail(i + 1);
        }

        // Act - This should trigger growth
        queue.EnqueueHead(0);
        queue.EnqueueHead(-1);

        // Assert - Verify the entire queue structure is correct
        Assert.Equal(18, queue.Count);
        Assert.Equal(-1, queue.DequeueHead());
        Assert.Equal(0, queue.DequeueHead());
        Assert.Equal(1, queue.DequeueHead());
        Assert.Equal(2, queue.DequeueHead());
        // Verify we can dequeue all elements (3 to 16, then nothing else)
        for (int i = 3; i <= 16; i++)
        {
            queue.DequeueHead();
        }
        Assert.Empty(queue);
    }

    [Fact]
    public void EnqueueTail_TriggersGrowth_MaintainsCorrectOrder()
    {
        // Arrange
        var queue = new PooledQueue<int>(16);
        for (int i = 0; i < 16; i++)
        {
            queue.EnqueueHead(i + 1);
        }

        // Act - This should trigger growth
        queue.EnqueueTail(17);
        queue.EnqueueTail(18);

        // Assert - Verify the entire queue structure is correct
        Assert.Equal(18, queue.Count);
        Assert.Equal(16, queue.DequeueHead());
        Assert.Equal(15, queue.DequeueHead());
        Assert.Equal(14, queue.DequeueHead());
        // Verify we can dequeue all elements in correct order (16, 15, 14, ..., 1)
        for (int i = 13; i >= 1; i--)
        {
            queue.DequeueHead();
        }
        Assert.Equal(17, queue.DequeueHead());
        Assert.Equal(18, queue.DequeueHead());
        Assert.Empty(queue);
    }

    [Fact]
    public void MixedEnqueue_TriggersGrowth_MaintainsCorrectStructure()
    {
        // Arrange
        var queue = new PooledQueue<int>(16);
        for (int i = 0; i < 16; i++)
        {
            queue.Enqueue(i + 1);
        }

        // Act - Mixed operations that trigger growth
        queue.EnqueueHead(0);      // Triggers growth
        queue.EnqueueTail(17);     // Add to tail after growth
        queue.EnqueueHead(-1);     // Add to head after growth
        queue.EnqueueTail(18);     // Add to tail after growth

        // Assert - Verify complete structure: -1, 0, 1, 2, ..., 16, 17, 18
        Assert.Equal(20, queue.Count);
        Assert.Equal(-1, queue.DequeueHead());
        Assert.Equal(0, queue.DequeueHead());
        Assert.Equal(1, queue.DequeueHead());
        Assert.Equal(2, queue.DequeueHead());
        // Verify the middle elements
        for (int i = 3; i <= 16; i++)
        {
            queue.DequeueHead();
        }
        Assert.Equal(17, queue.DequeueHead());
        Assert.Equal(18, queue.DequeueHead());
        Assert.Empty(queue);
    }

    [Fact]
    public void EnqueueHeadAfterWrapping_TriggersGrowth_MaintainsCorrectStructure()
    {
        // Arrange - Create a wrapped state
        var queue = new PooledQueue<int>(16);
        for (int i = 0; i < 16; i++)
        {
            queue.Enqueue(i + 1);
        }
        queue.Dequeue();  // Creates space at front
        queue.Dequeue();  // More space at front

        // Now queue has 14 elements with some space at front (wrapped state)
        // Act - Add to head, should trigger growth
        queue.EnqueueHead(0);   // This fills one more slot
        queue.EnqueueHead(-1);  // Should trigger growth

        // Assert - After growth, order should be correct
        Assert.Equal(16, queue.Count);
        Assert.Equal(-1, queue.DequeueHead());
        Assert.Equal(0, queue.DequeueHead());
        Assert.Equal(3, queue.DequeueHead());
        // Verify remaining elements
        for (int i = 4; i <= 16; i++)
        {
            queue.DequeueHead();
        }
        Assert.Empty(queue);
    }

    [Fact]
    public void GrowthWithDequeOperations_MaintainsCorrectStructure()
    {
        // Arrange
        var queue = new PooledQueue<int>(16);
        for (int i = 0; i < 16; i++)
        {
            queue.Enqueue(i + 1);
        }

        // Act - Growth followed by deque operations
        queue.EnqueueHead(0);   // Triggers growth: 17 elements
        queue.DequeueTail();    // Remove from tail (16): 16 elements
        queue.EnqueueTail(17);  // Add to tail: 17 elements
        queue.EnqueueHead(-1);  // Add to head: 18 elements
        queue.DequeueHead();    // Remove from head (-1): 17 elements

        // Assert
        Assert.Equal(17, queue.Count);
        Assert.Equal(0, queue.DequeueHead());
        Assert.Equal(1, queue.DequeueHead());
        // Verify middle elements (2-15)
        for (int i = 2; i <= 15; i++)
        {
            queue.DequeueHead();
        }
        Assert.Equal(17, queue.DequeueHead()); // Last element is 17
        Assert.Empty(queue);
    }

    [Fact]
    public void TryHeadAndTryTail_AfterGrowth_ReturnCorrectValues()
    {
        // Arrange
        var queue = new PooledQueue<int>(16);
        for (int i = 0; i < 16; i++)
        {
            queue.Enqueue(i + 1);
        }

        // Act - Trigger growth
        queue.EnqueueHead(0);
        queue.EnqueueTail(17);

        // Assert - TryHead and TryTail should work correctly after growth
        Assert.Equal(18, queue.Count);
        Assert.True(queue.TryHead(out int headValue));
        Assert.Equal(0, headValue);
        Assert.True(queue.TryTail(out int tailValue));
        Assert.Equal(17, tailValue);

        // Verify full structure
        Assert.Equal(0, queue.DequeueHead());
        Assert.Equal(1, queue.DequeueHead());
        // Verify middle elements
        for (int i = 2; i <= 15; i++)
        {
            queue.DequeueHead();
        }
        Assert.Equal(16, queue.DequeueHead());
        Assert.Equal(17, queue.DequeueTail());
    }
}
