// Copyright (c) 2026, RickyYC and Contributors. All rights reserved.
// Distributed under the MIT Software License, Version 1.0.

namespace Faster.Collections.Pooled.Tests.Dictionary;

public class TryGetValueTests
{
    [Fact]
    public void TryGetValue_ExistingKey_ReturnsTrueAndValue()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        dictionary[1] = "one";
        dictionary[2] = "two";

        // Act
        var result = dictionary.TryGetValue(1, out var value);

        // Assert
        Assert.True(result);
        Assert.Equal("one", value);
    }

    [Fact]
    public void TryGetValue_NonExistingKey_ReturnsFalseAndDefaultValue()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        dictionary[1] = "one";

        // Act
        var result = dictionary.TryGetValue(99, out var value);

        // Assert
        Assert.False(result);
        Assert.Null(value);
    }

    [Fact]
    public void TryGetValue_EmptyDictionary_ReturnsFalse()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();

        // Act
        var result = dictionary.TryGetValue(1, out var value);

        // Assert
        Assert.False(result);
        Assert.Null(value);
    }

    [Fact]
    public void TryGetValue_IntegerValue_ReturnsCorrectValue()
    {
        // Arrange
        var dictionary = new FasterDictionary<string, int>();
        dictionary["apple"] = 42;

        // Act
        var result = dictionary.TryGetValue("apple", out var value);

        // Assert
        Assert.True(result);
        Assert.Equal(42, value);
    }

    [Fact]
    public void TryGetValue_NullValue_ReturnsNull()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string?>();
        dictionary[1] = null!;

        // Act
        var result = dictionary.TryGetValue(1, out var value);

        // Assert
        Assert.True(result);
        Assert.Null(value);
    }
}
