// Copyright (c) 2026, RickyYC and Contributors. All rights reserved.
// Distributed under the MIT Software License, Version 1.0.

namespace Faster.Collections.Pooled.Tests.Dictionary;

public class ContainsKeyTests
{
    [Fact]
    public void ContainsKey_ExistingKey_ReturnsTrue()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        dictionary[1] = "one";
        dictionary[2] = "two";

        // Act
        var result = dictionary.ContainsKey(2);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void ContainsKey_NonExistingKey_ReturnsFalse()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        dictionary[1] = "one";
        dictionary[2] = "two";

        // Act
        var result = dictionary.ContainsKey(99);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void ContainsKey_EmptyDictionary_ReturnsFalse()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();

        // Act
        var result = dictionary.ContainsKey(1);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void ContainsKey_StringKey_CaseSensitive()
    {
        // Arrange
        var dictionary = new FasterDictionary<string, int>();
        dictionary["Apple"] = 1;
        dictionary["Banana"] = 2;

        // Act
        var result1 = dictionary.ContainsKey("Apple");
        var result2 = dictionary.ContainsKey("apple");

        // Assert
        Assert.True(result1);
        Assert.False(result2);
    }
}
