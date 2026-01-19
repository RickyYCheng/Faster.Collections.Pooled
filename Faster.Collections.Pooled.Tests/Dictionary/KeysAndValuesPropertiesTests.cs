// Copyright (c) 2026, RickyYC and Contributors. All rights reserved.
// Distributed under the MIT Software License, Version 1.0.

namespace Faster.Collections.Pooled.Tests.Dictionary;

public class KeysAndValuesPropertiesTests
{
    [Fact]
    public void Keys_EmptyDictionary_ReturnsEmptyCollection()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();

        // Act
        var keys = dictionary.Keys;

        // Assert
        Assert.Empty(keys);
    }

    [Fact]
    public void Keys_PopulatedDictionary_ReturnsAllKeys()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        dictionary[1] = "one";
        dictionary[2] = "two";
        dictionary[3] = "three";

        // Act
        var keys = dictionary.Keys;

        // Assert
        Assert.Equal(3, keys.Count);
        Assert.Contains(1, keys);
        Assert.Contains(2, keys);
        Assert.Contains(3, keys);
    }

    [Fact]
    public void Values_EmptyDictionary_ReturnsEmptyCollection()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();

        // Act
        var values = dictionary.Values;

        // Assert
        Assert.Empty(values);
    }

    [Fact]
    public void Values_PopulatedDictionary_ReturnsAllValues()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        dictionary[1] = "one";
        dictionary[2] = "two";
        dictionary[3] = "three";

        // Act
        var values = dictionary.Values;

        // Assert
        Assert.Equal(3, values.Count);
        Assert.Contains("one", values);
        Assert.Contains("two", values);
        Assert.Contains("three", values);
    }

    [Fact]
    public void Keys_MultipleAccesses_ReturnsSameInstance()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();

        // Act
        var keys1 = dictionary.Keys;
        var keys2 = dictionary.Keys;

        // Assert
        Assert.Same(keys1, keys2);
    }

    [Fact]
    public void Values_MultipleAccesses_ReturnsSameInstance()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();

        // Act
        var values1 = dictionary.Values;
        var values2 = dictionary.Values;

        // Assert
        Assert.Same(values1, values2);
    }
}
