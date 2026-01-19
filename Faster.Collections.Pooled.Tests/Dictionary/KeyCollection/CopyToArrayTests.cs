// Copyright (c) 2026, RickyYC and Contributors. All rights reserved.
// Distributed under the MIT Software License, Version 1.0.

using System.Collections;

namespace Faster.Collections.Pooled.Tests.Dictionary.Dictionary.KeyCollection;

public class CopyToArrayTests
{
    [Fact]
    public void CopyTo_EmptyDictionary_CopiesZeroItems()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        var array = new object[5];

        // Act
        ((ICollection)dictionary.Keys).CopyTo(array, 0);

        // Assert
        Assert.Null(array[0]);
        Assert.Null(array[1]);
    }

    [Fact]
    public void CopyTo_SingleItem_CopiesItem()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        dictionary[1] = "one";
        var array = new object[5];

        // Act
        ((ICollection)dictionary.Keys).CopyTo(array, 0);

        // Assert
        Assert.Equal(1, array[0]);
        Assert.Null(array[1]);
    }

    [Fact]
    public void CopyTo_MultipleItems_CopiesAllItems()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        dictionary[1] = "one";
        dictionary[2] = "two";
        dictionary[3] = "three";
        var array = new object[5];

        // Act
        ((ICollection)dictionary.Keys).CopyTo(array, 0);

        // Assert
        Assert.Equal(1, array[0]);
        Assert.Equal(2, array[1]);
        Assert.Equal(3, array[2]);
        Assert.Null(array[3]);
    }

    [Fact]
    public void CopyTo_WithOffset_StartsAtCorrectIndex()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        dictionary[1] = "one";
        dictionary[2] = "two";
        dictionary[3] = "three";
        var array = new object[10];

        // Act
        ((ICollection)dictionary.Keys).CopyTo(array, 3);

        // Assert
        Assert.Null(array[0]);
        Assert.Null(array[1]);
        Assert.Null(array[2]);
        Assert.Equal(1, array[3]);
        Assert.Equal(2, array[4]);
        Assert.Equal(3, array[5]);
        Assert.Null(array[6]);
    }

    [Fact]
    public void CopyTo_NullArray_ThrowsArgumentNullException()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        dictionary[1] = "one";

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => ((ICollection)dictionary.Keys).CopyTo(null!, 0));
    }

    [Fact]
    public void CopyTo_NegativeIndex_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        dictionary[1] = "one";
        var array = new object[5];

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => ((ICollection)dictionary.Keys).CopyTo(array, -1));
    }

    [Fact]
    public void CopyTo_InsufficientSpace_ThrowsArgumentException()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        dictionary[1] = "one";
        dictionary[2] = "two";
        dictionary[3] = "three";
        var array = new object[2];

        // Act & Assert
        Assert.Throws<ArgumentException>(() => ((ICollection)dictionary.Keys).CopyTo(array, 0));
    }

    [Fact]
    public void CopyTo_InsufficientSpaceWithOffset_ThrowsArgumentException()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        dictionary[1] = "one";
        dictionary[2] = "two";
        var array = new object[5];

        // Act & Assert
        Assert.Throws<ArgumentException>(() => ((ICollection)dictionary.Keys).CopyTo(array, 4));
    }

    [Fact]
    public void CopyTo_PreservesOriginalArrayElements()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        dictionary[1] = "one";
        dictionary[2] = "two";
        var array = new object[5];
        array[0] = 100;
        array[4] = 200;

        // Act
        ((ICollection)dictionary.Keys).CopyTo(array, 1);

        // Assert
        Assert.Equal(100, array[0]);
        Assert.Equal(1, array[1]);
        Assert.Equal(2, array[2]);
        Assert.Null(array[3]);
        Assert.Equal(200, array[4]);
    }

    [Fact]
    public void CopyTo_StringKeys()
    {
        // Arrange
        var dictionary = new FasterDictionary<string, int>();
        dictionary["apple"] = 1;
        dictionary["banana"] = 2;
        dictionary["cherry"] = 3;
        var array = new object[5];

        // Act
        ((ICollection)dictionary.Keys).CopyTo(array, 0);

        // Assert
        Assert.Equal("apple", array[0]);
        Assert.Equal("banana", array[1]);
        Assert.Equal("cherry", array[2]);
        Assert.Null(array[3]);
        Assert.Null(array[4]);
    }

    [Fact]
    public void CopyTo_ValueTypeKeys()
    {
        // Arrange
        var dictionary = new FasterDictionary<long, string>();
        dictionary[100L] = "one";
        dictionary[200L] = "two";
        var array = new object[5];

        // Act
        ((ICollection)dictionary.Keys).CopyTo(array, 0);

        // Assert
        Assert.Equal(100L, array[0]);
        Assert.Equal(200L, array[1]);
        Assert.Null(array[2]);
    }
}
