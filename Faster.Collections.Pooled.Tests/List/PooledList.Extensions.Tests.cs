// Copyright (c) 2026, RickyYC and Contributors. All rights reserved.
// Distributed under the MIT Software License, Version 1.0.

namespace Faster.Collections.Pooled.Tests.List;

public class PooledListExtensionsTests
{
    [Fact]
    public void Constructor_FromReadOnlySpan_CreatesListWithCorrectItems()
    {
        // Arrange
        var source = new ReadOnlySpan<int>([1, 2, 3, 4, 5]);

        // Act
        var list = new PooledList<int>(source);

        // Assert
        Assert.Equal(5, list.Count);
        Assert.Equal(1, list[0]);
        Assert.Equal(2, list[1]);
        Assert.Equal(3, list[2]);
        Assert.Equal(4, list[3]);
        Assert.Equal(5, list[4]);
    }

    [Fact]
    public void Constructor_FromEmptySpan_CreatesEmptyList()
    {
        // Arrange
        var source = ReadOnlySpan<int>.Empty;

        // Act
        var list = new PooledList<int>(source);

        // Assert
        Assert.Empty(list);
    }

    [Fact]
    public void AsSpan_ReturnsCorrectSpan()
    {
        // Arrange
        var list = new PooledList<int>();
        list.Add(1);
        list.Add(2);
        list.Add(3);

        // Act
        var span = list.AsSpan();

        // Assert
        Assert.Equal(3, span.Length);
        Assert.Equal(1, span[0]);
        Assert.Equal(2, span[1]);
        Assert.Equal(3, span[2]);
    }

    [Fact]
    public void AsSpan_EmptyList_ReturnsEmptySpan()
    {
        // Arrange
        var list = new PooledList<int>();

        // Act
        var span = list.AsSpan();

        // Assert
        Assert.Equal(0, span.Length);
    }

    [Fact]
    public void IndexerWithRange_ReturnsCorrectSlice()
    {
        // Arrange
        var list = new PooledList<int>();
        list.AddRange([1, 2, 3, 4, 5, 6, 7, 8, 9, 10]);

        // Act
        var slice = list[2..5];

        // Assert
        Assert.Equal(3, slice.Length);
        Assert.Equal(3, slice[0]);
        Assert.Equal(4, slice[1]);
        Assert.Equal(5, slice[2]);
    }

    [Fact]
    public void IndexerWithRange_FromStart_ReturnsCorrectSlice()
    {
        // Arrange
        var list = new PooledList<int>();
        list.AddRange([1, 2, 3, 4, 5]);

        // Act
        var slice = list[..3];

        // Assert
        Assert.Equal(3, slice.Length);
        Assert.Equal(1, slice[0]);
        Assert.Equal(2, slice[1]);
        Assert.Equal(3, slice[2]);
    }

    [Fact]
    public void IndexerWithRange_ToEnd_ReturnsCorrectSlice()
    {
        // Arrange
        var list = new PooledList<int>();
        list.AddRange([1, 2, 3, 4, 5]);

        // Act
        var slice = list[2..];

        // Assert
        Assert.Equal(3, slice.Length);
        Assert.Equal(3, slice[0]);
        Assert.Equal(4, slice[1]);
        Assert.Equal(5, slice[2]);
    }

    [Fact]
    public void IndexerWithRange_All_ReturnsCorrectSlice()
    {
        // Arrange
        var list = new PooledList<int>();
        list.AddRange([1, 2, 3, 4, 5]);

        // Act
        var slice = list[..];

        // Assert
        Assert.Equal(5, slice.Length);
        Assert.Equal(1, slice[0]);
        Assert.Equal(5, slice[4]);
    }

    [Fact]
    public void AddRange_WithReadOnlySpan_AddsAllItems()
    {
        // Arrange
        var list = new PooledList<int>();
        var source = new ReadOnlySpan<int>([1, 2, 3]);

        // Act
        list.AddRange(source);

        // Assert
        Assert.Equal(3, list.Count);
        Assert.Equal(1, list[0]);
        Assert.Equal(2, list[1]);
        Assert.Equal(3, list[2]);
    }

    [Fact]
    public void AddRange_WithEmptySpan_DoesNotModifyList()
    {
        // Arrange
        var list = new PooledList<int>();
        list.Add(1);
        list.Add(2);
        var countBefore = list.Count;

        // Act
        list.AddRange(ReadOnlySpan<int>.Empty);

        // Assert
        Assert.Equal(countBefore, list.Count);
        Assert.Equal(2, list.Count);
    }

    [Fact]
    public void AddRange_ToNonEmptyList_AppendsItems()
    {
        // Arrange
        var list = new PooledList<int>();
        list.Add(1);
        list.Add(2);
        var source = new ReadOnlySpan<int>([3, 4, 5]);

        // Act
        list.AddRange(source);

        // Assert
        Assert.Equal(5, list.Count);
        Assert.Equal(1, list[0]);
        Assert.Equal(2, list[1]);
        Assert.Equal(3, list[2]);
        Assert.Equal(4, list[3]);
        Assert.Equal(5, list[4]);
    }

    [Fact]
    public void AddRange_LargeSpan_TriggersGrowth()
    {
        // Arrange
        var list = new PooledList<int>(4);
        list.Add(1);
        list.Add(2);
        var source = new ReadOnlySpan<int>([3, 4, 5, 6, 7, 8]);

        // Act
        list.AddRange(source);

        // Assert
        Assert.Equal(8, list.Count);
        for (int i = 0; i < 8; i++)
        {
            Assert.Equal(i + 1, list[i]);
        }
    }

    [Fact]
    public void AddRange_NullList_ThrowsArgumentNullException()
    {
        // Arrange
        PooledList<int>? list = null;

        // Act & Assert
        // Cannot use ReadOnlySpan in lambda, so we use a helper method
        Assert.Throws<ArgumentNullException>(() =>
        {
            var data = new int[] { 1, 2, 3 };
            var source = new ReadOnlySpan<int>(data);
            list!.AddRange(source);
        });
    }

    [Fact]
    public void CopyTo_WithSpan_CopiesAllItems()
    {
        // Arrange
        var list = new PooledList<int>();
        list.AddRange([1, 2, 3, 4, 5]);
        var destination = new int[5];

        // Act
        list.CopyTo(destination);

        // Assert
        Assert.Equal([1, 2, 3, 4, 5], destination);
    }

    [Fact]
    public void CopyTo_EmptyList_CopiesNothing()
    {
        // Arrange
        var list = new PooledList<int>();
        var destination = new int[3];

        // Act
        list.CopyTo(destination);

        // Assert
        Assert.Equal([0, 0, 0], destination);
    }

    [Fact]
    public void CopyTo_LargerDestination_CopiesItemsToStart()
    {
        // Arrange
        var list = new PooledList<int>();
        list.AddRange([1, 2, 3]);
        var destination = new int[5];

        // Act
        list.CopyTo(destination);

        // Assert
        Assert.Equal(1, destination[0]);
        Assert.Equal(2, destination[1]);
        Assert.Equal(3, destination[2]);
        Assert.Equal(0, destination[3]);
        Assert.Equal(0, destination[4]);
    }

    [Fact]
    public void CopyTo_NullList_ThrowsArgumentNullException()
    {
        // Arrange
        PooledList<int>? list = null;
        var destination = new int[5];

        // Act & Assert
        // Use the extension method class directly to avoid null-forgiving operator
        Assert.Throws<ArgumentNullException>(() => PooledCollectionsExtensions.CopyTo(list, destination));
    }

    [Fact]
    public void InsertRange_AtBeginning_InsertsItems()
    {
        // Arrange
        var list = new PooledList<int>();
        list.AddRange([3, 4, 5]);
        var source = new ReadOnlySpan<int>([1, 2]);

        // Act
        list.InsertRange(0, source);

        // Assert
        Assert.Equal(5, list.Count);
        Assert.Equal(1, list[0]);
        Assert.Equal(2, list[1]);
        Assert.Equal(3, list[2]);
        Assert.Equal(4, list[3]);
        Assert.Equal(5, list[4]);
    }

    [Fact]
    public void InsertRange_InMiddle_InsertsItems()
    {
        // Arrange
        var list = new PooledList<int>();
        list.AddRange([1, 4, 5]);
        var source = new ReadOnlySpan<int>([2, 3]);

        // Act
        list.InsertRange(1, source);

        // Assert
        Assert.Equal(5, list.Count);
        Assert.Equal(1, list[0]);
        Assert.Equal(2, list[1]);
        Assert.Equal(3, list[2]);
        Assert.Equal(4, list[3]);
        Assert.Equal(5, list[4]);
    }

    [Fact]
    public void InsertRange_AtEnd_AppendsItems()
    {
        // Arrange
        var list = new PooledList<int>();
        list.AddRange([1, 2, 3]);
        var source = new ReadOnlySpan<int>([4, 5]);

        // Act
        list.InsertRange(3, source);

        // Assert
        Assert.Equal(5, list.Count);
        Assert.Equal(1, list[0]);
        Assert.Equal(2, list[1]);
        Assert.Equal(3, list[2]);
        Assert.Equal(4, list[3]);
        Assert.Equal(5, list[4]);
    }

    [Fact]
    public void InsertRange_EmptySpan_DoesNotModifyList()
    {
        // Arrange
        var list = new PooledList<int>();
        list.AddRange([1, 2, 3]);
        var countBefore = list.Count;

        // Act
        list.InsertRange(1, ReadOnlySpan<int>.Empty);

        // Assert
        Assert.Equal(countBefore, list.Count);
        Assert.Equal(3, list.Count);
    }

    [Fact]
    public void InsertRange_LargeSpan_TriggersGrowth()
    {
        // Arrange
        var list = new PooledList<int>(4);
        list.AddRange([1, 2]);
        var source = new ReadOnlySpan<int>([3, 4, 5, 6, 7, 8]);

        // Act
        list.InsertRange(2, source);

        // Assert
        Assert.Equal(8, list.Count);
        Assert.Equal(1, list[0]);
        Assert.Equal(2, list[1]);
        // Items 3-8 were inserted at index 2
        Assert.Equal(3, list[2]);
        Assert.Equal(4, list[3]);
        Assert.Equal(5, list[4]);
        Assert.Equal(6, list[5]);
        Assert.Equal(7, list[6]);
        Assert.Equal(8, list[7]);
    }

    [Fact]
    public void InsertRange_InvalidIndex_ThrowsIndexOutOfRangeException()
    {
        // Arrange
        var list = new PooledList<int>();
        list.AddRange([1, 2, 3]);

        // Act & Assert
        // Cannot use ReadOnlySpan in lambda, so we use a helper method
        Assert.Throws<IndexOutOfRangeException>(() =>
        {
            var data = new int[] { 4, 5 };
            var source = new ReadOnlySpan<int>(data);
            list.InsertRange(5, source);
        });
        Assert.Throws<IndexOutOfRangeException>(() =>
        {
            var data = new int[] { 4, 5 };
            var source = new ReadOnlySpan<int>(data);
            list.InsertRange(-1, source);
        });
    }

    [Fact]
    public void InsertRange_NullList_ThrowsArgumentNullException()
    {
        // Arrange
        PooledList<int>? list = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
        {
            var data = new int[] { 1, 2, 3 };
            var source = new ReadOnlySpan<int>(data);
            list!.InsertRange(0, source);
        });
    }

    [Fact]
    public void AsSpan_CanBeModified_UpdatesList()
    {
        // Arrange
        var list = new PooledList<int>();
        list.AddRange([1, 2, 3, 4, 5]);

        // Act
        var span = list.AsSpan();
        span[0] = 10;
        span[2] = 30;

        // Assert
        Assert.Equal(10, list[0]);
        Assert.Equal(2, list[1]);
        Assert.Equal(30, list[2]);
        Assert.Equal(4, list[3]);
        Assert.Equal(5, list[4]);
    }

    [Fact]
    public void AsSpan_NullList_ThrowsArgumentNullException()
    {
        // Arrange
        PooledList<int>? list = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => list!.AsSpan());
    }

    [Fact]
    public void CombinedOperations_WorkCorrectly()
    {
        // Arrange
        var list = new PooledList<int>();
        var data1 = new int[] { 10, 20 };

        // Act - Multiple operations
        list.AddRange([1, 2, 3]);
        list.InsertRange(1, new ReadOnlySpan<int>(data1));
        list.AddRange([4, 5]);

        // Assert
        Assert.Equal(7, list.Count);
        Assert.Equal(1, list[0]);
        Assert.Equal(10, list[1]);
        Assert.Equal(20, list[2]);
        Assert.Equal(2, list[3]);
        Assert.Equal(3, list[4]);
        Assert.Equal(4, list[5]);
        Assert.Equal(5, list[6]);
    }

    [Fact]
    public void RangeIndexerWithSlicing_MultipleOperations()
    {
        // Arrange
        var list = new PooledList<int>();
        list.AddRange([1, 2, 3, 4, 5, 6, 7, 8, 9, 10]);

        // Act
        var slice1 = list.AsSpan()[0..3];    // [1, 2, 3]
        var slice2 = list.AsSpan()[3..7];    // [4, 5, 6, 7]
        var slice3 = list.AsSpan()[7..];     // [8, 9, 10]

        // Assert
        Assert.Equal(3, slice1.Length);
        Assert.Equal(4, slice2.Length);
        Assert.Equal(3, slice3.Length);
        Assert.Equal(1, slice1[0]);
        Assert.Equal(4, slice2[0]);
        Assert.Equal(8, slice3[0]);
    }

    [Fact]
    public void ConstructorAndAddRange_WithStrings_WorksCorrectly()
    {
        // Arrange
        var data = new string[] { "a", "b", "c" };
        var source = new ReadOnlySpan<string>(data);
        var data2 = new string[] { "d", "e" };

        // Act
        var list = new PooledList<string>(source);
        list.AddRange(new ReadOnlySpan<string>(data2));

        // Assert
        Assert.Equal(5, list.Count);
        Assert.Equal("a", list[0]);
        Assert.Equal("b", list[1]);
        Assert.Equal("c", list[2]);
        Assert.Equal("d", list[3]);
        Assert.Equal("e", list[4]);
    }

    [Fact]
    public void InsertRange_WithReferenceTypes_WorksCorrectly()
    {
        // Arrange
        var list = new PooledList<string>();
        list.AddRange(["a", "d"]);
        var data = new string[] { "b", "c" };

        // Act
        list.InsertRange(1, new ReadOnlySpan<string>(data));

        // Assert
        Assert.Equal(4, list.Count);
        Assert.Equal("a", list[0]);
        Assert.Equal("b", list[1]);
        Assert.Equal("c", list[2]);
        Assert.Equal("d", list[3]);
    }
}
