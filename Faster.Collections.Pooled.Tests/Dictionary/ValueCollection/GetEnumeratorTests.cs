// Copyright (c) 2026, RickyYC and Contributors. All rights reserved.
// Distributed under the MIT Software License, Version 1.0.

namespace Faster.Collections.Pooled.Tests.Dictionary.ValueCollection;

public class GetEnumeratorTests
{
    [Fact]
    public void GetEnumerator_EmptyDictionary_ReturnsEmptyEnumerator()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();

        // Act
        var enumerator = dictionary.Values.GetEnumerator();

        // Assert
        Assert.False(enumerator.MoveNext());
    }

    [Fact]
    public void GetEnumerator_SingleItem_EnumeratesAllItems()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        dictionary[1] = "one";

        // Act
        var enumerator = dictionary.Values.GetEnumerator();
        var hasFirst = enumerator.MoveNext();
        var current = enumerator.Current;
        var hasSecond = enumerator.MoveNext();

        // Assert
        Assert.True(hasFirst);
        Assert.Equal("one", current);
        Assert.False(hasSecond);
    }

    [Fact]
    public void GetEnumerator_MultipleItems_EnumeratesAllItems()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        dictionary[1] = "one";
        dictionary[2] = "two";
        dictionary[3] = "three";

        // Act
        var enumerator = dictionary.Values.GetEnumerator();
        var items = new System.Collections.Generic.List<string>();
        while (enumerator.MoveNext())
        {
            items.Add(enumerator.Current);
        }

        // Assert
        Assert.Equal(3, items.Count);
        Assert.Contains("one", items);
        Assert.Contains("two", items);
        Assert.Contains("three", items);
    }

    [Fact]
    public void GetEnumerator_CurrentBeforeMoveNext_ThrowsIndexOutOfRangeException()
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
    public void GetEnumerator_CurrentAfterEnumerationEnd_ReturnsLastElement()
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
    public void GetEnumerator_MultipleEnumerations_AllWork()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        dictionary[1] = "one";
        dictionary[2] = "two";

        // Act
        var enumerator1 = dictionary.Values.GetEnumerator();
        var items1 = new System.Collections.Generic.List<string>();
        while (enumerator1.MoveNext())
        {
            items1.Add(enumerator1.Current);
        }

        var enumerator2 = dictionary.Values.GetEnumerator();
        var items2 = new System.Collections.Generic.List<string>();
        while (enumerator2.MoveNext())
        {
            items2.Add(enumerator2.Current);
        }

        // Assert
        Assert.Equal(2, items1.Count);
        Assert.Equal(2, items2.Count);
    }

    [Fact]
    public void GetEnumerator_GenericInterface_ReturnsEnumerator()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        dictionary[1] = "one";
        dictionary[2] = "two";

        // Act
        var enumerator = ((System.Collections.Generic.IEnumerable<string>)dictionary.Values).GetEnumerator();
        var items = new System.Collections.Generic.List<string>();
        while (enumerator.MoveNext())
        {
            items.Add(enumerator.Current);
        }

        // Assert
        Assert.Equal(2, items.Count);
    }

    [Fact]
    public void GetEnumerator_NonGenericInterface_ReturnsEnumerator()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        dictionary[1] = "one";
        dictionary[2] = "two";

        // Act
        var enumerator = ((System.Collections.IEnumerable)dictionary.Values).GetEnumerator();
        var items = new System.Collections.Generic.List<string>();
        while (enumerator.MoveNext())
        {
            items.Add((string)enumerator.Current!);
        }

        // Assert
        Assert.Equal(2, items.Count);
    }
}
