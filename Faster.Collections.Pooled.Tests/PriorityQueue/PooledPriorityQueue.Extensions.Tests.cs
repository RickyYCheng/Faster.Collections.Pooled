// Copyright (c) 2026, RickyYC and Contributors. All rights reserved.
// Distributed under the MIT Software License, Version 1.0.

namespace Faster.Collections.Pooled.Tests.PriorityQueue;

public class PooledPriorityQueueExtensionsTests
{
    [Fact]
    public void Constructor_FromSpan_CreatesQueueWithCorrectItems()
    {
        // Arrange
        var data = new (int Element, int Priority)[]
        {
            (1, 3),
            (2, 1),
            (3, 2)
        };
        var source = new ReadOnlySpan<(int Element, int Priority)>(data);

        // Act
        var queue = new PooledPriorityQueue<int, int>(source);

        // Assert
        Assert.Equal(3, queue.Count);
        // Items should be dequeued in priority order (lowest priority number first)
        Assert.Equal(2, queue.Dequeue()); // Priority 1
        Assert.Equal(3, queue.Dequeue()); // Priority 2
        Assert.Equal(1, queue.Dequeue()); // Priority 3
    }

    [Fact]
    public void Constructor_FromSpanWithComparer_CreatesQueueWithCorrectItems()
    {
        // Arrange
        var data = new (string Element, int Priority)[]
        {
            ("low", 3),
            ("high", 1),
            ("medium", 2)
        };
        var source = new ReadOnlySpan<(string Element, int Priority)>(data);
        var comparer = Comparer<int>.Default;

        // Act
        var queue = new PooledPriorityQueue<string, int>(source, comparer);

        // Assert
        Assert.Equal(3, queue.Count);
        Assert.Equal("high", queue.Dequeue());
        Assert.Equal("medium", queue.Dequeue());
        Assert.Equal("low", queue.Dequeue());
    }

    [Fact]
    public void Constructor_FromEmptySpan_CreatesEmptyQueue()
    {
        // Arrange
        var source = ReadOnlySpan<(int Element, int Priority)>.Empty;

        // Act
        var queue = new PooledPriorityQueue<int, int>(source);

        // Assert
        Assert.Equal(0, queue.Count);
    }

    [Fact]
    public void Constructor_FromSingleElement_WorksCorrectly()
    {
        // Arrange
        var data = new (int Element, int Priority)[] { (42, 1) };
        var source = new ReadOnlySpan<(int Element, int Priority)>(data);

        // Act
        var queue = new PooledPriorityQueue<int, int>(source);

        // Assert
        Assert.Equal(1, queue.Count);
        Assert.Equal(42, queue.Dequeue());
    }

    [Fact]
    public void Constructor_WithSamePriority_WorksCorrectly()
    {
        // Arrange
        var data = new (int Element, int Priority)[]
        {
            (1, 1),
            (2, 1),
            (3, 1)
        };
        var source = new ReadOnlySpan<(int Element, int Priority)>(data);

        // Act
        var queue = new PooledPriorityQueue<int, int>(source);

        // Assert
        Assert.Equal(3, queue.Count);
        // All have same priority, so all should be dequeued (order not guaranteed)
        var items = new HashSet<int>();
        items.Add(queue.Dequeue());
        items.Add(queue.Dequeue());
        items.Add(queue.Dequeue());

        Assert.Equal(3, items.Count);
        Assert.Contains(1, items);
        Assert.Contains(2, items);
        Assert.Contains(3, items);
    }

    [Fact]
    public void Constructor_LargeSpan_WorksCorrectly()
    {
        // Arrange
        var data = new (int Element, int Priority)[100];
        for (int i = 0; i < data.Length; i++)
        {
            data[i] = (i, i % 10); // Priority cycles 0-9
        }
        var source = new ReadOnlySpan<(int Element, int Priority)>(data);

        // Act
        var queue = new PooledPriorityQueue<int, int>(source);

        // Assert
        Assert.Equal(100, queue.Count);

        // Verify all items are in the queue
        var dequeuedItems = new HashSet<int>();
        while (queue.Count > 0)
        {
            dequeuedItems.Add(queue.Dequeue());
        }

        Assert.Equal(100, dequeuedItems.Count);
        // All items from 0-99 should have been dequeued
        for (int i = 0; i < 100; i++)
        {
            Assert.Contains(i, dequeuedItems);
        }
    }

    [Fact]
    public void Constructor_WithCustomComparer_WorksCorrectly()
    {
        // Arrange
        var data = new (int Element, int Priority)[]
        {
            (1, 1),
            (2, 2),
            (3, 3)
        };
        var source = new ReadOnlySpan<(int Element, int Priority)>(data);
        var comparer = Comparer<int>.Create((x, y) => y.CompareTo(x)); // Reverse order

        // Act
        var queue = new PooledPriorityQueue<int, int>(source, comparer);

        // Assert
        Assert.Equal(3, queue.Count);
        // With reverse comparer, highest priority comes first
        Assert.Equal(3, queue.Dequeue());
        Assert.Equal(2, queue.Dequeue());
        Assert.Equal(1, queue.Dequeue());
    }

    [Fact]
    public void Constructor_ThenEnqueue_WorksCorrectly()
    {
        // Arrange
        var data = new (int Element, int Priority)[]
        {
            (1, 2),
            (2, 1)
        };
        var source = new ReadOnlySpan<(int Element, int Priority)>(data);

        // Act
        var queue = new PooledPriorityQueue<int, int>(source);
        queue.Enqueue(3, 3);

        // Assert
        Assert.Equal(3, queue.Count);
        Assert.Equal(2, queue.Dequeue()); // Priority 1
        Assert.Equal(1, queue.Dequeue()); // Priority 2
        Assert.Equal(3, queue.Dequeue()); // Priority 3
    }

    [Fact]
    public void Constructor_PeekOperation_WorksCorrectly()
    {
        // Arrange
        var data = new (int Element, int Priority)[]
        {
            (10, 3),
            (20, 1),
            (30, 2)
        };
        var source = new ReadOnlySpan<(int Element, int Priority)>(data);

        // Act
        var queue = new PooledPriorityQueue<int, int>(source);

        // Assert
        Assert.Equal(20, queue.Peek()); // Highest priority (lowest number)
        Assert.Equal(3, queue.Count); // Verify no item was removed
    }

    [Fact]
    public void Constructor_TryPeek_WorksCorrectly()
    {
        // Arrange
        var data = new (int Element, int Priority)[]
        {
            (1, 2),
            (2, 1)
        };
        var source = new ReadOnlySpan<(int Element, int Priority)>(data);

        // Act
        var queue = new PooledPriorityQueue<int, int>(source);

        // Assert
        Assert.True(queue.TryPeek(out int element, out int priority));
        Assert.Equal(2, element);
        Assert.Equal(1, priority);
        Assert.Equal(2, queue.Count); // Verify no item was removed
    }

    [Fact]
    public void Constructor_WithNullComparer_UsesDefaultComparer()
    {
        // Arrange
        var data = new (string Element, int Priority)[]
        {
            ("a", 3),
            ("b", 1),
            ("c", 2)
        };
        var source = new ReadOnlySpan<(string Element, int Priority)>(data);

        // Act
        var queue = new PooledPriorityQueue<string, int>(source, null);

        // Assert
        Assert.Equal(3, queue.Count);
        Assert.Equal("b", queue.Dequeue());
        Assert.Equal("c", queue.Dequeue());
        Assert.Equal("a", queue.Dequeue());
    }

    [Fact]
    public void Constructor_ClearAndReuse_WorksCorrectly()
    {
        // Arrange
        var data = new (int Element, int Priority)[]
        {
            (1, 1),
            (2, 2)
        };
        var source = new ReadOnlySpan<(int Element, int Priority)>(data);
        var queue = new PooledPriorityQueue<int, int>(source);

        // Act
        queue.Clear();
        queue.Enqueue(10, 1);
        queue.Enqueue(20, 2);

        // Assert
        Assert.Equal(2, queue.Count);
        Assert.Equal(10, queue.Dequeue());
        Assert.Equal(20, queue.Dequeue());
    }

    [Fact]
    public void Constructor_WithDuplicateElements_WorksCorrectly()
    {
        // Arrange
        var data = new (int Element, int Priority)[]
        {
            (1, 1),
            (1, 2),
            (1, 1)
        };
        var source = new ReadOnlySpan<(int Element, int Priority)>(data);

        // Act
        var queue = new PooledPriorityQueue<int, int>(source);

        // Assert
        Assert.Equal(3, queue.Count);
        Assert.Equal(1, queue.Dequeue()); // First priority 1
        Assert.Equal(1, queue.Dequeue()); // Second priority 1
        Assert.Equal(1, queue.Dequeue()); // Priority 2
    }

    [Fact]
    public void Constructor_WithNegativePriorities_WorksCorrectly()
    {
        // Arrange
        var data = new (int Element, int Priority)[]
        {
            (1, 0),
            (2, -1),
            (3, 1)
        };
        var source = new ReadOnlySpan<(int Element, int Priority)>(data);

        // Act
        var queue = new PooledPriorityQueue<int, int>(source);

        // Assert
        Assert.Equal(3, queue.Count);
        Assert.Equal(2, queue.Dequeue()); // Priority -1 (highest)
        Assert.Equal(1, queue.Dequeue()); // Priority 0
        Assert.Equal(3, queue.Dequeue()); // Priority 1 (lowest)
    }

    [Fact]
    public void Constructor_WithReferenceTypeElements_WorksCorrectly()
    {
        // Arrange
        var data = new (string Element, int Priority)[]
        {
            ("first", 3),
            ("second", 1),
            ("third", 2)
        };
        var source = new ReadOnlySpan<(string Element, int Priority)>(data);

        // Act
        var queue = new PooledPriorityQueue<string, int>(source);

        // Assert
        Assert.Equal(3, queue.Count);
        Assert.Equal("second", queue.Dequeue());
        Assert.Equal("third", queue.Dequeue());
        Assert.Equal("first", queue.Dequeue());
    }

    [Fact]
    public void Constructor_WithReferenceTypePriorities_WorksCorrectly()
    {
        // Arrange
        var data = new (int Element, string Priority)[]
        {
            (1, "low"),
            (2, "high"),
            (3, "medium")
        };
        var source = new ReadOnlySpan<(int Element, string Priority)>(data);

        // Act
        var queue = new PooledPriorityQueue<int, string>(source);

        // Assert
        Assert.Equal(3, queue.Count);
        // String comparison: "high" < "low" < "medium" (alphabetical order)
        Assert.Equal(2, queue.Dequeue()); // "high"
        Assert.Equal(1, queue.Dequeue()); // "low"
        Assert.Equal(3, queue.Dequeue()); // "medium"
    }

    [Fact]
    public void Constructor_ThenEnqueueDequeueMixed_WorksCorrectly()
    {
        // Arrange
        var data = new (int Element, int Priority)[]
        {
            (1, 2),
            (2, 1)
        };
        var source = new ReadOnlySpan<(int Element, int Priority)>(data);

        // Act
        var queue = new PooledPriorityQueue<int, int>(source);
        queue.Enqueue(3, 3);
        var first = queue.Dequeue();
        queue.Enqueue(0, 0);
        var second = queue.Dequeue();
        var third = queue.Dequeue();
        var fourth = queue.Dequeue();

        // Assert
        Assert.Equal(2, first);  // Priority 1 (original)
        Assert.Equal(0, second); // Priority 0 (new)
        Assert.Equal(1, third);  // Priority 2 (original)
        Assert.Equal(3, fourth); // Priority 3 (new)
    }

    [Fact]
    public void Constructor_UnorderedItems_WorksCorrectly()
    {
        // Arrange
        var data = new (int Element, int Priority)[]
        {
            (1, 2),
            (2, 1),
            (3, 3)
        };
        var source = new ReadOnlySpan<(int Element, int Priority)>(data);

        // Act
        var queue = new PooledPriorityQueue<int, int>(source);
        var unorderedItems = queue.UnorderedItems.ToList();

        // Assert
        Assert.Equal(3, unorderedItems.Count);
        // The enumerator doesn't guarantee order, but all elements should be present
        Assert.Contains((1, 2), unorderedItems);
        Assert.Contains((2, 1), unorderedItems);
        Assert.Contains((3, 3), unorderedItems);
    }

    [Fact]
    public void Constructor_WithLargeSpanAndOperations_WorksCorrectly()
    {
        // Arrange
        var data = new (int Element, int Priority)[50];
        for (int i = 0; i < data.Length; i++)
        {
            data[i] = (i, i % 5);
        }
        var source = new ReadOnlySpan<(int Element, int Priority)>(data);

        // Act
        var queue = new PooledPriorityQueue<int, int>(source);
        queue.Enqueue(100, 0);
        queue.Enqueue(200, 4);

        // Assert
        Assert.Equal(52, queue.Count);

        // Collect all items to verify they were all processed
        var items = new List<int>();
        while (queue.Count > 0)
        {
            items.Add(queue.Dequeue());
        }

        Assert.Equal(52, items.Count);
        // Verify all original items 0-49 are present
        for (int i = 0; i < 50; i++)
        {
            Assert.Contains(i, items);
        }
        // Verify the added items are present
        Assert.Contains(100, items);
        Assert.Contains(200, items);

        // First items should be priority 0 items (0, 5, 10, 15, 20, 25, 30, 35, 40, 45, and 100)
        var priorityZeroItems = items.Take(11).ToList();
        Assert.Equal(11, priorityZeroItems.Count);
        Assert.Contains(0, priorityZeroItems);
        Assert.Contains(5, priorityZeroItems);
        Assert.Contains(100, priorityZeroItems);
    }

    [Fact]
    public void Constructor_ThenTrimExcess_WorksCorrectly()
    {
        // Arrange
        var data = new (int Element, int Priority)[]
        {
            (1, 1),
            (2, 2),
            (3, 3),
            (4, 4)
        };
        var source = new ReadOnlySpan<(int Element, int Priority)>(data);
        var queue = new PooledPriorityQueue<int, int>(source);

        // Act
        queue.Dequeue();
        queue.Dequeue();
        queue.TrimExcess();

        // Assert
        Assert.Equal(2, queue.Count);
        Assert.Equal(3, queue.Dequeue());
        Assert.Equal(4, queue.Dequeue());
    }

    [Fact]
    public void Constructor_EnsureCapacity_WorksCorrectly()
    {
        // Arrange
        var data = new (int Element, int Priority)[]
        {
            (1, 1),
            (2, 2)
        };
        var source = new ReadOnlySpan<(int Element, int Priority)>(data);

        // Act
        var queue = new PooledPriorityQueue<int, int>(source);
        queue.EnsureCapacity(100);

        // Assert
        Assert.True(queue.Capacity >= 100);
        Assert.Equal(2, queue.Count);
    }

    [Fact]
    public void Constructor_WithCustomComparerAndReverseOrder_WorksCorrectly()
    {
        // Arrange
        var data = new (string Element, int Priority)[]
        {
            ("a", 10),
            ("b", 5),
            ("c", 15)
        };
        var source = new ReadOnlySpan<(string Element, int Priority)>(data);
        var comparer = Comparer<int>.Create((x, y) => y.CompareTo(x)); // Descending

        // Act
        var queue = new PooledPriorityQueue<string, int>(source, comparer);

        // Assert
        Assert.Equal(3, queue.Count);
        // With descending comparer, highest number comes first
        Assert.Equal("c", queue.Dequeue()); // Priority 15
        Assert.Equal("a", queue.Dequeue()); // Priority 10
        Assert.Equal("b", queue.Dequeue()); // Priority 5
    }

    [Fact]
    public void Constructor_TryDequeue_WorksCorrectly()
    {
        // Arrange
        var data = new (int Element, int Priority)[]
        {
            (1, 1),
            (2, 2)
        };
        var source = new ReadOnlySpan<(int Element, int Priority)>(data);

        // Act
        var queue = new PooledPriorityQueue<int, int>(source);
        var success = queue.TryDequeue(out int element, out int priority);

        // Assert
        Assert.True(success);
        Assert.Equal(1, element);
        Assert.Equal(1, priority);
        Assert.Equal(1, queue.Count);
    }

    [Fact]
    public void Constructor_DequeueEnqueue_WorksCorrectly()
    {
        // Arrange
        var data = new (int Element, int Priority)[]
        {
            (1, 1),
            (2, 2),
            (3, 3)
        };
        var source = new ReadOnlySpan<(int Element, int Priority)>(data);

        // Act
        var queue = new PooledPriorityQueue<int, int>(source);
        var result = queue.DequeueEnqueue(4, 0);

        // Assert
        Assert.Equal(1, result); // The element with priority 1
        Assert.Equal(3, queue.Count);
        Assert.Equal(4, queue.Peek()); // New element with priority 0 is now at top
    }

    [Fact]
    public void Constructor_EnqueueDequeue_WorksCorrectly()
    {
        // Arrange
        var data = new (int Element, int Priority)[]
        {
            (1, 1),
            (2, 2)
        };
        var source = new ReadOnlySpan<(int Element, int Priority)>(data);

        // Act
        var queue = new PooledPriorityQueue<int, int>(source);
        // EnqueueDequeue adds the element and then removes the minimum priority element
        // So it adds (3, 0), then dequeues (3, 0) since it has the highest priority (lowest number)
        var result = queue.EnqueueDequeue(3, 0);

        // Assert
        // The result should be 3 because it was added and then immediately removed
        Assert.Equal(3, result);
        Assert.Equal(2, queue.Count); // Still has (1,1) and (2,2)
        Assert.Equal(1, queue.Peek()); // Element 1 with priority 1 is now at top
    }
}
