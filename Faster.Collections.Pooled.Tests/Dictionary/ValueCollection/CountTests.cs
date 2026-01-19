// Copyright (c) 2026, RickyYC and Contributors. All rights reserved.
// Distributed under the MIT Software License, Version 1.0.

namespace Faster.Collections.Pooled.Tests.Dictionary.ValueCollection;

public class CountTests
{
    [Fact]
    public void Count_EmptyDictionary_ReturnsZero()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();

        // Act
        var count = dictionary.Values.Count;

        // Assert
        Assert.Equal(0, count);
    }

    [Fact]
    public void Count_SingleItem_ReturnsOne()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        dictionary[1] = "one";

        // Act
        var count = dictionary.Values.Count;

        // Assert
        Assert.Equal(1, count);
    }

    [Fact]
    public void Count_MultipleItems_ReturnsCorrectCount()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        for (int i = 0; i < 100; i++)
        {
            dictionary[i] = $"value{i}";
        }

        // Act
        var count = dictionary.Values.Count;

        // Assert
        Assert.Equal(100, count);
    }

    [Fact]
    public void Count_AfterAdd_ReturnsUpdatedCount()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        dictionary[1] = "one";
        dictionary[2] = "two";

        // Act
        var initialCount = dictionary.Values.Count;
        dictionary[3] = "three";
        var finalCount = dictionary.Values.Count;

        // Assert
        Assert.Equal(2, initialCount);
        Assert.Equal(3, finalCount);
    }

    [Fact]
    public void Count_AfterRemove_ReturnsUpdatedCount()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        dictionary[1] = "one";
        dictionary[2] = "two";
        dictionary[3] = "three";

        // Act
        var initialCount = dictionary.Values.Count;
        dictionary.Remove(2);
        var finalCount = dictionary.Values.Count;

        // Assert
        Assert.Equal(3, initialCount);
        Assert.Equal(2, finalCount);
    }

    [Fact]
    public void Count_MatchesDictionaryCount()
    {
        // Arrange
        var dictionary = new FasterDictionary<string, int>();
        dictionary["apple"] = 1;
        dictionary["banana"] = 2;
        dictionary["cherry"] = 3;

        // Act & Assert
        Assert.Equal(dictionary.Count, dictionary.Values.Count);
    }
}
