// Copyright (c) 2026, RickyYC and Contributors. All rights reserved.
// Distributed under the MIT Software License, Version 1.0.

namespace Faster.Collections.Pooled.Tests.Dictionary;

public class ConstructorTests
{
    [Fact]
    public void DefaultConstructor_CreatesEmptyDictionary()
    {
        // Act
        var dictionary = new FasterDictionary<int, string>();

        // Assert
        Assert.Equal(0, dictionary.Count);
    }

    [Fact]
    public void ConstructorWithCapacity_CreatesEmptyDictionary()
    {
        // Act
        var dictionary = new FasterDictionary<int, string>(100);

        // Assert
        Assert.Equal(0, dictionary.Count);
    }

    [Fact]
    public void ConstructorWithCapacityAndLoadFactor_CreatesEmptyDictionary()
    {
        // Act
        var dictionary = new FasterDictionary<int, string>(50, 0.75);

        // Assert
        Assert.Equal(0, dictionary.Count);
    }

    [Fact]
    public void DefaultConstructor_AllowsAddingItems()
    {
        // Arrange & Act
        var dictionary = new FasterDictionary<int, string>();
        dictionary[1] = "one";
        dictionary[2] = "two";

        // Assert
        Assert.Equal(2, dictionary.Count);
    }

    [Fact]
    public void ConstructorWithCapacity_AllowsAddingItems()
    {
        // Arrange & Act
        var dictionary = new FasterDictionary<int, string>(5);
        dictionary[1] = "one";
        dictionary[2] = "two";
        dictionary[3] = "three";

        // Assert
        Assert.Equal(3, dictionary.Count);
    }

    [Fact]
    public void ConstructorWithCapacityAndLoadFactor_AllowsAddingItems()
    {
        // Arrange & Act
        var dictionary = new FasterDictionary<int, string>(5, 0.9);
        for (int i = 0; i < 20; i++)
        {
            dictionary[i] = $"value{i}";
        }

        // Assert
        Assert.Equal(20, dictionary.Count);
    }
}
