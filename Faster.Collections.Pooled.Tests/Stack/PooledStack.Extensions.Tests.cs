// Copyright (c) 2026, RickyYC and Contributors. All rights reserved.
// Distributed under the MIT Software License, Version 1.0.

namespace Faster.Collections.Pooled.Tests.Stack;

public class PooledStackExtensionsTests
{
    [Fact]
    public void Constructor_FromReadOnlySpan_CreatesStackWithCorrectItems()
    {
        // Arrange
        var source = new ReadOnlySpan<int>([1, 2, 3, 4, 5]);

        // Act
        var stack = new PooledStack<int>(source);

        // Assert
        Assert.Equal(5, stack.Count);
        // Stack is LIFO, so items are popped in reverse order
        Assert.Equal(5, stack.Pop());
        Assert.Equal(4, stack.Pop());
        Assert.Equal(3, stack.Pop());
        Assert.Equal(2, stack.Pop());
        Assert.Equal(1, stack.Pop());
        Assert.Empty(stack);
    }

    [Fact]
    public void Constructor_FromEmptySpan_CreatesEmptyStack()
    {
        // Arrange
        var source = ReadOnlySpan<int>.Empty;

        // Act
        var stack = new PooledStack<int>(source);

        // Assert
        Assert.Empty(stack);
    }

    [Fact]
    public void Constructor_FromSingleElementSpan_WorksCorrectly()
    {
        // Arrange
        var source = new ReadOnlySpan<int>([42]);

        // Act
        var stack = new PooledStack<int>(source);

        // Assert
        Assert.Single(stack);
        Assert.Equal(42, stack.Pop());
    }

    [Fact]
    public void Constructor_WithReferenceTypes_WorksCorrectly()
    {
        // Arrange
        var source = new ReadOnlySpan<string>(["a", "b", "c"]);

        // Act
        var stack = new PooledStack<string>(source);

        // Assert
        Assert.Equal(3, stack.Count);
        Assert.Equal("c", stack.Pop());
        Assert.Equal("b", stack.Pop());
        Assert.Equal("a", stack.Pop());
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
        var stack = new PooledStack<int>(source);

        // Assert
        Assert.Equal(1000, stack.Count);
        Assert.Equal(1000, stack.Pop());
        Assert.Equal(999, stack.Pop());
        Assert.Equal(998, stack.Pop());
    }

    [Fact]
    public void Constructor_PeekOperation_WorksCorrectly()
    {
        // Arrange
        var source = new ReadOnlySpan<int>([1, 2, 3]);

        // Act
        var stack = new PooledStack<int>(source);

        // Assert
        Assert.Equal(3, stack.Peek());
        Assert.Equal(3, stack.Count); // Verify no item was removed
    }

    [Fact]
    public void Constructor_TryPeek_WorksCorrectly()
    {
        // Arrange
        var source = new ReadOnlySpan<int>([10, 20, 30]);

        // Act
        var stack = new PooledStack<int>(source);

        // Assert
        Assert.True(stack.TryPeek(out int value));
        Assert.Equal(30, value);
        Assert.Equal(3, stack.Count); // Verify no item was removed
    }

    [Fact]
    public void Constructor_TryPop_WorksCorrectly()
    {
        // Arrange
        var source = new ReadOnlySpan<int>([1, 2, 3]);

        // Act
        var stack = new PooledStack<int>(source);
        var success = stack.TryPop(out int result);

        // Assert
        Assert.True(success);
        Assert.Equal(3, result);
        Assert.Equal(2, stack.Count);
    }

    [Fact]
    public void Constructor_WithNullStrings_HandlesCorrectly()
    {
        // Arrange
        var source = new ReadOnlySpan<string?>(["a", null, "b"]);

        // Act
        var stack = new PooledStack<string?>(source);

        // Assert
        Assert.Equal(3, stack.Count);
        Assert.Equal("b", stack.Pop());
        Assert.Null(stack.Pop());
        Assert.Equal("a", stack.Pop());
    }

    [Fact]
    public void Constructor_FollowedByPush_WorksCorrectly()
    {
        // Arrange
        var source = new ReadOnlySpan<int>([1, 2, 3]);

        // Act
        var stack = new PooledStack<int>(source);
        stack.Push(4);
        stack.Push(5);

        // Assert
        Assert.Equal(5, stack.Count);
        Assert.Equal(5, stack.Pop());
        Assert.Equal(4, stack.Pop());
        Assert.Equal(3, stack.Pop());
    }

    [Fact]
    public void Constructor_ClearAndReuse_WorksCorrectly()
    {
        // Arrange
        var source = new ReadOnlySpan<int>([1, 2, 3]);
        var stack = new PooledStack<int>(source);

        // Act
        stack.Clear();
        stack.Push(10);
        stack.Push(20);

        // Assert
        Assert.Equal(2, stack.Count);
        Assert.Equal(20, stack.Pop());
        Assert.Equal(10, stack.Pop());
    }

    [Fact]
    public void Constructor_CopyToSpan_WorksCorrectly()
    {
        // Arrange
        var source = new ReadOnlySpan<int>([1, 2, 3]);
        var stack = new PooledStack<int>(source);

        // Act
        var array = new int[3];
        stack.CopyTo(array, 0);

        // Assert
        // The order in the array is the reverse of insertion order
        Assert.Equal(3, array[0]);
        Assert.Equal(2, array[1]);
        Assert.Equal(1, array[2]);
    }

    [Fact]
    public void Constructor_WithEmptyStackAfterOperations_WorksCorrectly()
    {
        // Arrange
        var source = new ReadOnlySpan<int>([1, 2, 3]);
        var stack = new PooledStack<int>(source);

        // Act - Pop all items
        stack.Pop();
        stack.Pop();
        stack.Pop();

        // Assert
        Assert.Empty(stack);
        Assert.False(stack.TryPop(out _));
        Assert.False(stack.TryPeek(out _));
    }

    [Fact]
    public void Constructor_ToArray_WorksCorrectly()
    {
        // Arrange
        var source = new ReadOnlySpan<int>([1, 2, 3, 4, 5]);

        // Act
        var stack = new PooledStack<int>(source);
        var array = stack.ToArray();

        // Assert
        Assert.Equal(5, array.Length);
        // The array should have items in LIFO order
        Assert.Equal(5, array[0]);
        Assert.Equal(4, array[1]);
        Assert.Equal(3, array[2]);
        Assert.Equal(2, array[3]);
        Assert.Equal(1, array[4]);
    }

    [Fact]
    public void Constructor_WithDuplicates_WorksCorrectly()
    {
        // Arrange
        var source = new ReadOnlySpan<int>([1, 1, 2, 2, 3, 3]);

        // Act
        var stack = new PooledStack<int>(source);

        // Assert
        Assert.Equal(6, stack.Count);
        Assert.Equal(3, stack.Pop());
        Assert.Equal(3, stack.Pop());
        Assert.Equal(2, stack.Pop());
        Assert.Equal(2, stack.Pop());
        Assert.Equal(1, stack.Pop());
        Assert.Equal(1, stack.Pop());
    }

    [Fact]
    public void Constructor_Enumerator_WorksCorrectly()
    {
        // Arrange
        var source = new ReadOnlySpan<int>([1, 2, 3]);

        // Act
        var stack = new PooledStack<int>(source);
        var result = stack.ToList();

        // Assert
        Assert.Equal(3, result.Count);
        // Enumerator should return items in LIFO order
        Assert.Equal(3, result[0]);
        Assert.Equal(2, result[1]);
        Assert.Equal(1, result[2]);
    }

    [Fact]
    public void Constructor_Contains_WorksCorrectly()
    {
        // Arrange
        var source = new ReadOnlySpan<int>([1, 2, 3, 4, 5]);

        // Act
        var stack = new PooledStack<int>(source);

        // Assert
        Assert.True(stack.Contains(3));
        Assert.True(stack.Contains(5));
        Assert.False(stack.Contains(10));
    }

    [Fact]
    public void Constructor_PopAllThenPush_WorksCorrectly()
    {
        // Arrange
        var source = new ReadOnlySpan<int>([1, 2, 3]);
        var stack = new PooledStack<int>(source);

        // Act
        stack.Pop();
        stack.Pop();
        stack.Pop();
        stack.Push(100);
        stack.Push(200);

        // Assert
        Assert.Equal(2, stack.Count);
        Assert.Equal(200, stack.Pop());
        Assert.Equal(100, stack.Pop());
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
        var stack = new PooledStack<int>(source);
        stack.Push(0);

        // Assert
        Assert.Equal(101, stack.Count);
        Assert.Equal(0, stack.Pop());
        Assert.Equal(100, stack.Pop());
        Assert.Equal(99, stack.Pop());
    }

    [Fact]
    public void Constructor_ThenTrimExcess_WorksCorrectly()
    {
        // Arrange
        var source = new ReadOnlySpan<int>([1, 2, 3, 4, 5, 6, 7, 8]);
        var stack = new PooledStack<int>(source);

        // Act
        for (int i = 0; i < 5; i++)
        {
            stack.Pop();
        }
        stack.TrimExcess();

        // Assert
        Assert.Equal(3, stack.Count);
        Assert.Equal(3, stack.Pop());
        Assert.Equal(2, stack.Pop());
        Assert.Equal(1, stack.Pop());
    }

    [Fact]
    public void Constructor_MixedPeekAndPop_WorksCorrectly()
    {
        // Arrange
        var source = new ReadOnlySpan<int>([1, 2, 3, 4, 5]);

        // Act
        var stack = new PooledStack<int>(source);
        var peek1 = stack.Peek();
        var pop1 = stack.Pop();
        var peek2 = stack.Peek();
        var pop2 = stack.Pop();

        // Assert
        Assert.Equal(5, peek1);
        Assert.Equal(5, pop1);
        Assert.Equal(4, peek2);
        Assert.Equal(4, pop2);
        Assert.Equal(3, stack.Count);
    }

    [Fact]
    public void Constructor_PeekAfterMultiplePushes_WorksCorrectly()
    {
        // Arrange
        var source = new ReadOnlySpan<int>([1, 2, 3]);
        var stack = new PooledStack<int>(source);

        // Act
        stack.Push(4);
        stack.Push(5);

        // Assert
        Assert.Equal(5, stack.Peek());
        Assert.Equal(5, stack.Count);
    }

    [Fact]
    public void Constructor_CopyToWithOffset_WorksCorrectly()
    {
        // Arrange
        var source = new ReadOnlySpan<int>([1, 2, 3]);
        var stack = new PooledStack<int>(source);

        // Act
        var array = new int[5];
        stack.CopyTo(array, 2);

        // Assert
        Assert.Equal(0, array[0]);
        Assert.Equal(0, array[1]);
        Assert.Equal(3, array[2]);
        Assert.Equal(2, array[3]);
        Assert.Equal(1, array[4]);
    }

    [Fact]
    public void Constructor_ContainsAfterOperations_WorksCorrectly()
    {
        // Arrange
        var source = new ReadOnlySpan<int>([1, 2, 3, 4, 5]);
        var stack = new PooledStack<int>(source);

        // Act
        stack.Pop();
        stack.Pop();
        stack.Push(10);

        // Assert
        Assert.True(stack.Contains(10));
        Assert.True(stack.Contains(3));
        Assert.False(stack.Contains(5));
        Assert.False(stack.Contains(100));
    }

    [Fact]
    public void Constructor_WithCustomClass_WorksCorrectly()
    {
        // Arrange
        var items = new (string Name, int Value)[]
        {
            ("A", 1),
            ("B", 2),
            ("C", 3)
        };
        var source = new ReadOnlySpan<(string Name, int Value)>(items);

        // Act
        var stack = new PooledStack<(string Name, int Value)>(source);

        // Assert
        Assert.Equal(3, stack.Count);
        var item = stack.Pop();
        Assert.Equal("C", item.Name);
        Assert.Equal(3, item.Value);
    }
}
