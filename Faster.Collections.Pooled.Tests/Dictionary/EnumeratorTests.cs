// Copyright (c) 2026, RickyYC and Contributors. All rights reserved.
// Distributed under the MIT Software License, Version 1.0.

namespace Faster.Collections.Pooled.Tests.Dictionary;

public class EnumeratorTests
{
    [Fact]
    public void Current_BeforeMoveNext_ThrowsIndexOutOfRangeException()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        dictionary[1] = "one";
        var enumerator = ((System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<int, string>>)dictionary).GetEnumerator();

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
    public void Current_AfterEnumerationEnd_ReturnsLastElement()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        dictionary[1] = "one";
        dictionary[2] = "two";
        var enumerator = ((System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<int, string>>)dictionary).GetEnumerator();
        enumerator.MoveNext();
        enumerator.MoveNext();

        // Act
        enumerator.MoveNext(); // Move past the end
        var current = enumerator.Current;

        // Assert
        Assert.Equal(2, current.Key);
        Assert.Equal("two", current.Value);
    }

    [Fact]
    public void MoveNext_EmptyDictionary_ReturnsFalse()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        var enumerator = ((System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<int, string>>)dictionary).GetEnumerator();

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
        var enumerator = ((System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<int, string>>)dictionary).GetEnumerator();

        // Act
        var firstMove = enumerator.MoveNext();
        var secondMove = enumerator.MoveNext();

        // Assert
        Assert.True(firstMove);
        Assert.False(secondMove);
    }

    [Fact]
    public void Reset_AfterEnumeration_StartsOver()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        dictionary[1] = "one";
        dictionary[2] = "two";
        var enumerator = ((System.Collections.IEnumerable)dictionary).GetEnumerator();

        // Act
        enumerator.MoveNext();
        enumerator.MoveNext();
        enumerator.MoveNext(); // Move past end
        enumerator.Reset();

        var canMoveAgain = enumerator.MoveNext();

        // Assert
        Assert.True(canMoveAgain);
    }

    [Fact]
    public void Dispose_DoesNotThrow()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        dictionary[1] = "one";
        var enumerator = ((System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<int, string>>)dictionary).GetEnumerator();
        enumerator.MoveNext();

        // Act & Assert
        enumerator.Dispose(); // Should not throw
    }

    [Fact]
    public void Entry_Property_ReturnsDictionaryEntry()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        dictionary[1] = "one";
        var enumerator = (System.Collections.IDictionaryEnumerator)((System.Collections.IDictionary)dictionary).GetEnumerator();
        enumerator.MoveNext();

        // Act
        var entry = enumerator.Entry;

        // Assert
        Assert.Equal(1, entry.Key);
        Assert.Equal("one", entry.Value);
    }

    [Fact]
    public void Key_Property_ReturnsCurrentKey()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        dictionary[42] = "answer";
        var enumerator = (System.Collections.IDictionaryEnumerator)((System.Collections.IDictionary)dictionary).GetEnumerator();
        enumerator.MoveNext();

        // Act
        var key = enumerator.Key;

        // Assert
        Assert.Equal(42, key);
    }

    [Fact]
    public void Value_Property_ReturnsCurrentValue()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        dictionary[1] = "test";
        var enumerator = (System.Collections.IDictionaryEnumerator)((System.Collections.IDictionary)dictionary).GetEnumerator();
        enumerator.MoveNext();

        // Act
        var value = enumerator.Value;

        // Assert
        Assert.Equal("test", value);
    }

    [Fact]
    public void IEnumerator_Current_ReturnsCurrent()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        dictionary[1] = "one";
        var enumerator = ((System.Collections.IEnumerable)dictionary).GetEnumerator();
        enumerator.MoveNext();

        // Act
        var current = enumerator.Current;

        // Assert
        var kvp = (System.Collections.Generic.KeyValuePair<int, string>)current!;
        Assert.Equal(1, kvp.Key);
        Assert.Equal("one", kvp.Value);
    }
}
