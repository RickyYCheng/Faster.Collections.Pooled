// Copyright (c) 2026, RickyYC and Contributors. All rights reserved.
// Distributed under the MIT Software License, Version 1.0.

namespace Faster.Collections.Pooled.Tests.Dictionary;

public class GetEnumeratorTests
{
    [Fact]
    public void GetEnumerator_EmptyDictionary_ReturnsEmptyEnumerator()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();

        // Act
        var enumerator = dictionary.GetEnumerator();

        // Assert
        Assert.False(enumerator.MoveNext());
    }

    [Fact]
    public void GetEnumerator_SingleItem_EnumeratesItem()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        dictionary[1] = "one";

        // Act
        var enumerator = dictionary.GetEnumerator();
        var hasFirst = enumerator.MoveNext();
        var current = enumerator.Current;
        var hasSecond = enumerator.MoveNext();

        // Assert
        Assert.True(hasFirst);
        Assert.Equal(1, current.Key);
        Assert.Equal("one", current.Value);
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
        var enumerator = dictionary.GetEnumerator();
        var count = 0;
        while (enumerator.MoveNext())
        {
            count++;
        }

        // Assert
        Assert.Equal(3, count);
    }

    [Fact]
    public void GetEnumerator_GenericInterface_ReturnsEnumerator()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        dictionary[1] = "one";
        dictionary[2] = "two";

        // Act
        var enumerator = ((System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<int, string>>)dictionary).GetEnumerator();
        var items = new System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<int, string>>();
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
        var enumerator = ((System.Collections.IEnumerable)dictionary).GetEnumerator();
        var count = 0;
        while (enumerator.MoveNext())
        {
            count++;
        }

        // Assert
        Assert.Equal(2, count);
    }

    [Fact]
    public void GetEnumerator_IDictionaryInterface_ReturnsDictionaryEnumerator()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        dictionary[1] = "one";
        dictionary[2] = "two";

        // Act
        var enumerator = ((System.Collections.IDictionary)dictionary).GetEnumerator();
        var entries = new System.Collections.Generic.List<System.Collections.DictionaryEntry>();
        while (enumerator.MoveNext())
        {
            entries.Add(enumerator.Entry);
        }

        // Assert
        Assert.Equal(2, entries.Count);
    }
}
