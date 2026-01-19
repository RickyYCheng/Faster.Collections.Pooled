// Copyright (c) 2026, RickyYC and Contributors. All rights reserved.
// Distributed under the MIT Software License, Version 1.0.

namespace Faster.Collections.Pooled.Tests.Dictionary.ValueCollection;

public class SpanEnumeratorTests
{
    [Fact]
    public void MoveNext_EmptyDictionary_ReturnsFalse()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        var enumerator = dictionary.Values.GetEnumerator();

        // Act
        var result = enumerator.MoveNext();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void MoveNext_SingleItem_ReturnsTrueThenFalse()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        dictionary[1] = "one";
        var enumerator = dictionary.Values.GetEnumerator();

        // Act
        var firstMove = enumerator.MoveNext();
        var secondMove = enumerator.MoveNext();

        // Assert
        Assert.True(firstMove);
        Assert.False(secondMove);
    }

    [Fact]
    public void MoveNext_MultipleItems_ReturnsTrueForEachItem()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        dictionary[1] = "one";
        dictionary[2] = "two";
        dictionary[3] = "three";
        var enumerator = dictionary.Values.GetEnumerator();

        // Act
        var move1 = enumerator.MoveNext();
        var move2 = enumerator.MoveNext();
        var move3 = enumerator.MoveNext();
        var move4 = enumerator.MoveNext();

        // Assert
        Assert.True(move1);
        Assert.True(move2);
        Assert.True(move3);
        Assert.False(move4);
    }

    [Fact]
    public void Current_BeforeMoveNext_ThrowsIndexOutOfRangeException()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        dictionary[1] = "one";
        var enumerator = dictionary.Values.GetEnumerator();

        // Act & Assert
        Exception? exception = null;
        try
        {
            _ = enumerator.Current;
        }
        catch (IndexOutOfRangeException ex)
        {
            exception = ex;
        }
        Assert.NotNull(exception);
    }

    [Fact]
    public void Current_AfterMoveNext_ReturnsCorrectValue()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        dictionary[1] = "one";
        dictionary[2] = "two";
        dictionary[3] = "three";
        var enumerator = dictionary.Values.GetEnumerator();

        // Act
        enumerator.MoveNext();
        var first = enumerator.Current;
        enumerator.MoveNext();
        var second = enumerator.Current;
        enumerator.MoveNext();
        var third = enumerator.Current;

        // Assert
        Assert.Equal("one", first);
        Assert.Equal("two", second);
        Assert.Equal("three", third);
    }

    [Fact]
    public void Current_AfterEnumerationEnd_ReturnsLastElement()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        dictionary[1] = "one";
        dictionary[2] = "two";
        var enumerator = dictionary.Values.GetEnumerator();
        enumerator.MoveNext();
        enumerator.MoveNext();

        // Act
        enumerator.MoveNext(); // Move past the end
        var current = enumerator.Current;

        // Assert
        Assert.Equal("two", current); // Returns last element, not throws
    }

    [Fact]
    public void IterateAllItems_ReturnsAllValues()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        for (int i = 1; i <= 100; i++)
        {
            dictionary[i] = $"value{i}";
        }
        var enumerator = dictionary.Values.GetEnumerator();

        // Act
        var count = 0;
        var values = new System.Collections.Generic.List<string>();
        while (enumerator.MoveNext())
        {
            values.Add(enumerator.Current);
            count++;
        }

        // Assert
        Assert.Equal(100, count);
        Assert.Equal(100, values.Count);
        for (int i = 1; i <= 100; i++)
        {
            Assert.Contains($"value{i}", values);
        }
    }

    [Fact]
    public void IterationOrder_MaintainsInsertionOrder()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        dictionary[5] = "five";
        dictionary[3] = "three";
        dictionary[1] = "one";
        dictionary[4] = "four";
        dictionary[2] = "two";
        var enumerator = dictionary.Values.GetEnumerator();

        // Act
        var values = new System.Collections.Generic.List<string>();
        while (enumerator.MoveNext())
        {
            values.Add(enumerator.Current);
        }

        // Assert
        Assert.Equal("five", values[0]);
        Assert.Equal("three", values[1]);
        Assert.Equal("one", values[2]);
        Assert.Equal("four", values[3]);
        Assert.Equal("two", values[4]);
    }

    [Fact]
    public void MultipleEnumerators_AllWorkIndependently()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        dictionary[1] = "one";
        dictionary[2] = "two";
        dictionary[3] = "three";

        // Act
        var enumerator1 = dictionary.Values.GetEnumerator();
        var enumerator2 = dictionary.Values.GetEnumerator();

        enumerator1.MoveNext();
        var first1 = enumerator1.Current;

        enumerator2.MoveNext();
        enumerator2.MoveNext();
        var first2 = enumerator2.Current;

        enumerator1.MoveNext();
        var second1 = enumerator1.Current;

        // Assert
        Assert.Equal("one", first1);
        Assert.Equal("two", first2);
        Assert.Equal("two", second1);
    }

    [Fact]
    public void Enumerator_RefStruct_CanBeUsedInForEach()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        dictionary[1] = "one";
        dictionary[2] = "two";
        dictionary[3] = "three";

        // Act
        var count = 0;
        foreach (var value in dictionary.Values)
        {
            count++;
        }

        // Assert
        Assert.Equal(3, count);
    }
}
