// Copyright (c) 2026, RickyYC and Contributors. All rights reserved.
// Distributed under the MIT Software License, Version 1.0.

namespace Faster.Collections.Pooled.Tests.Dictionary.ValueCollection;

public class ValueEnumeratorTests
{
    [Fact]
    public void Current_BeforeMoveNext_ThrowsIndexOutOfRangeException()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        dictionary[1] = "one";

        // Act & Assert
        Exception? exception = null;
        try
        {
            var enumerator = ((System.Collections.Generic.IEnumerable<string>)dictionary.Values).GetEnumerator();
            _ = enumerator.Current;
        }
        catch (IndexOutOfRangeException ex)
        {
            exception = ex;
        }
        Assert.NotNull(exception);
    }

    [Fact]
    public void Current_AfterEnumerationEnd_ReturnsLastElement()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        dictionary[1] = "one";
        dictionary[2] = "two";
        var enumerator = ((System.Collections.Generic.IEnumerable<string>)dictionary.Values).GetEnumerator();
        enumerator.MoveNext();
        enumerator.MoveNext();

        // Act
        enumerator.MoveNext(); // Move past the end
        var current = enumerator.Current;

        // Assert
        Assert.Equal("two", current); // Returns last element, not throws
    }

    [Fact]
    public void MoveNext_EmptyDictionary_ReturnsFalse()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        var enumerator = ((System.Collections.Generic.IEnumerable<string>)dictionary.Values).GetEnumerator();

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
        var enumerator = ((System.Collections.Generic.IEnumerable<string>)dictionary.Values).GetEnumerator();

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
        var enumerator = ((System.Collections.Generic.IEnumerable<string>)dictionary.Values).GetEnumerator();

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
    public void Reset_AfterEnumeration_StartsOver()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        dictionary[1] = "one";
        dictionary[2] = "two";

        // Act
        var enumerator = ((System.Collections.IEnumerable)dictionary.Values).GetEnumerator();
        enumerator.MoveNext();
        enumerator.MoveNext();
        enumerator.MoveNext(); // Move past end
        enumerator.Reset();

        var canMoveAgain = enumerator.MoveNext();

        // Assert
        Assert.True(canMoveAgain);
    }

    [Fact]
    public void Reset_AtBeginning_RemainsAtBeginning()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        dictionary[1] = "one";

        // Act
        var enumerator = ((System.Collections.IEnumerable)dictionary.Values).GetEnumerator();
        enumerator.Reset();
        var result = enumerator.MoveNext();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Dispose_DoesNotThrow()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        dictionary[1] = "one";

        // Act & Assert
        var enumerator = ((System.Collections.Generic.IEnumerable<string>)dictionary.Values).GetEnumerator();
        enumerator.MoveNext();
        enumerator.Dispose(); // Should not throw
    }

    [Fact]
    public void Enumerator_IterationOrder_MaintainsInsertionOrder()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        dictionary[1] = "one";
        dictionary[2] = "two";
        dictionary[3] = "three";
        var enumerator = ((System.Collections.Generic.IEnumerable<string>)dictionary.Values).GetEnumerator();

        // Act
        var items = new System.Collections.Generic.List<string>();
        while (enumerator.MoveNext())
        {
            items.Add(enumerator.Current);
        }

        // Assert
        Assert.Equal("one", items[0]);
        Assert.Equal("two", items[1]);
        Assert.Equal("three", items[2]);
    }

    [Fact]
    public void IEnumerator_Current_ReturnsSameAsGenericCurrent()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        dictionary[1] = "answer";
        var enumerator = ((System.Collections.IEnumerable)dictionary.Values).GetEnumerator();
        enumerator.MoveNext();

        // Act
        var genericCurrent = ((System.Collections.Generic.IEnumerator<string>)enumerator).Current;
        var nonGenericCurrent = enumerator.Current;

        // Assert
        Assert.Equal(genericCurrent, nonGenericCurrent);
    }
}
