// Copyright (c) 2026, RickyYC and Contributors. All rights reserved.
// Distributed under the MIT Software License, Version 1.0.

namespace Faster.Collections.Pooled.Tests.Dictionary.ValueCollection;

public class ContainsTests
{
    [Fact]
    public void Contains_ExistingValue_ReturnsTrue()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        dictionary[1] = "one";
        dictionary[2] = "two";
        dictionary[3] = "three";

        // Act
        var result = dictionary.Values.Contains("two");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Contains_NonExistingValue_ReturnsFalse()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        dictionary[1] = "one";
        dictionary[2] = "two";

        // Act
        var result = dictionary.Values.Contains("ninety-nine");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Contains_EmptyDictionary_ReturnsFalse()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();

        // Act
        var result = dictionary.Values.Contains("test");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Contains_DuplicateValues_ReturnsTrue()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        dictionary[1] = "same";
        dictionary[2] = "same";
        dictionary[3] = "different";

        // Act
        var result = dictionary.Values.Contains("same");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Contains_NullValue_WhenValueTypeIsNullable()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string?>();
        dictionary[1] = "test";
        dictionary[2] = null!;

        // Act
        var result = dictionary.Values.Contains(null);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Contains_AfterRemove_ReturnsFalse()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        dictionary[1] = "one";
        dictionary[2] = "two";

        // Act
        dictionary.Remove(1);
        var result = dictionary.Values.Contains("one");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Contains_AfterAdd_ReturnsTrue()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        dictionary[1] = "one";

        // Act
        dictionary[2] = "two";
        var result = dictionary.Values.Contains("two");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Contains_IntegerValues()
    {
        // Arrange
        var dictionary = new FasterDictionary<string, int>();
        dictionary["apple"] = 1;
        dictionary["banana"] = 2;
        dictionary["cherry"] = 3;

        // Act
        var result1 = dictionary.Values.Contains(2);
        var result2 = dictionary.Values.Contains(99);

        // Assert
        Assert.True(result1);
        Assert.False(result2);
    }

    [Fact]
    public void Contains_CustomValueType()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, Tuple<int, int>>();
        dictionary[1] = new Tuple<int, int>(1, 2);
        dictionary[2] = new Tuple<int, int>(3, 4);

        // Act
        var result1 = dictionary.Values.Contains(new Tuple<int, int>(1, 2));
        var result2 = dictionary.Values.Contains(new Tuple<int, int>(5, 6));

        // Assert
        Assert.True(result1);
        Assert.False(result2);
    }
}
