// Copyright (c) 2026, RickyYC and Contributors. All rights reserved.
// Distributed under the MIT Software License, Version 1.0.

using System.Collections;

namespace Faster.Collections.Pooled.Tests.Dictionary;

public class CopyToTests
{
    [Fact]
    public void CopyTo_KeyValuePairArray_EmptyDictionary()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        var array = new System.Collections.Generic.KeyValuePair<int, string>[5];

        // Act
        dictionary.CopyTo(array, 0);

        // Assert
        Assert.Equal(default, array[0]);
        Assert.Equal(default, array[1]);
    }

    [Fact]
    public void CopyTo_KeyValuePairArray_CopiesAllItems()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        dictionary[1] = "one";
        dictionary[2] = "two";
        dictionary[3] = "three";
        var array = new System.Collections.Generic.KeyValuePair<int, string>[5];

        // Act
        dictionary.CopyTo(array, 0);

        // Assert
        Assert.Equal(1, array[0].Key);
        Assert.Equal("one", array[0].Value);
        Assert.Equal(2, array[1].Key);
        Assert.Equal("two", array[1].Value);
        Assert.Equal(3, array[2].Key);
        Assert.Equal("three", array[2].Value);
    }

    [Fact]
    public void CopyTo_KeyValuePairArray_WithOffset()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        dictionary[1] = "one";
        dictionary[2] = "two";
        var array = new System.Collections.Generic.KeyValuePair<int, string>[5];

        // Act
        dictionary.CopyTo(array, 2);

        // Assert
        Assert.Equal(default, array[0]);
        Assert.Equal(default, array[1]);
        Assert.Equal(1, array[2].Key);
        Assert.Equal("one", array[2].Value);
    }

    [Fact]
    public void CopyTo_Array_EmptyDictionary()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        var array = new object[5];

        // Act
        ((ICollection)dictionary).CopyTo(array, 0);

        // Assert
        Assert.Null(array[0]);
        Assert.Null(array[1]);
    }

    [Fact]
    public void CopyTo_Array_CopiesAllItemsAsDictionaryEntries()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        dictionary[1] = "one";
        dictionary[2] = "two";
        var array = new object[5];

        // Act
        ((ICollection)dictionary).CopyTo(array, 0);

        // Assert
        var entry1 = (DictionaryEntry)array[0]!;
        var entry2 = (DictionaryEntry)array[1]!;
        Assert.Equal(1, entry1.Key);
        Assert.Equal("one", entry1.Value);
        Assert.Equal(2, entry2.Key);
        Assert.Equal("two", entry2.Value);
    }

    [Fact]
    public void CopyTo_KeyValuePairArray_NullArray_ThrowsArgumentNullException()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        dictionary[1] = "one";

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => dictionary.CopyTo(null!, 0));
    }

    [Fact]
    public void CopyTo_Array_NullArray_ThrowsArgumentNullException()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        dictionary[1] = "one";

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => ((ICollection)dictionary).CopyTo(null!, 0));
    }

    [Fact]
    public void CopyTo_KeyValuePairArray_NegativeIndex_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        var array = new System.Collections.Generic.KeyValuePair<int, string>[5];

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => dictionary.CopyTo(array, -1));
    }

    [Fact]
    public void CopyTo_Array_NegativeIndex_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        var array = new object[5];

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => ((ICollection)dictionary).CopyTo(array, -1));
    }

    [Fact]
    public void CopyTo_KeyValuePairArray_InsufficientSpace_ThrowsArgumentException()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        dictionary[1] = "one";
        dictionary[2] = "two";
        dictionary[3] = "three";
        var array = new System.Collections.Generic.KeyValuePair<int, string>[2];

        // Act & Assert
        Assert.Throws<ArgumentException>(() => dictionary.CopyTo(array, 0));
    }

    [Fact]
    public void CopyTo_Array_InsufficientSpace_ThrowsArgumentException()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        dictionary[1] = "one";
        dictionary[2] = "two";
        dictionary[3] = "three";
        var array = new object[2];

        // Act & Assert
        Assert.Throws<ArgumentException>(() => ((ICollection)dictionary).CopyTo(array, 0));
    }
}
