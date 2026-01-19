// Copyright (c) 2026, RickyYC and Contributors. All rights reserved.
// Distributed under the MIT Software License, Version 1.0.

namespace Faster.Collections.Pooled.Tests.Dictionary.Dictionary.KeyCollection;

public class ContainsTests
{
    [Fact]
    public void Contains_ExistingKey_ReturnsTrue()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        dictionary[1] = "one";
        dictionary[2] = "two";
        dictionary[3] = "three";

        // Act
        var result = dictionary.Keys.Contains(2);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Contains_NonExistingKey_ReturnsFalse()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        dictionary[1] = "one";
        dictionary[2] = "two";

        // Act
        var result = dictionary.Keys.Contains(99);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Contains_EmptyDictionary_ReturnsFalse()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();

        // Act
        var result = dictionary.Keys.Contains(1);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Contains_StringKeys_CaseSensitive()
    {
        // Arrange
        var dictionary = new FasterDictionary<string, int>();
        dictionary["Apple"] = 1;
        dictionary["Banana"] = 2;

        // Act
        var result1 = dictionary.Keys.Contains("Apple");
        var result2 = dictionary.Keys.Contains("apple");

        // Assert
        Assert.True(result1);
        Assert.False(result2);
    }

    [Fact]
    public void Contains_NullKey_WhenKeyTypeIsNullable()
    {
        // Arrange
        var dictionary = new FasterDictionary<string?, int>();
        dictionary["test"] = 1;
        dictionary[null!] = 0;

        // Act
        var result = dictionary.Keys.Contains(null);

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
        var result = dictionary.Keys.Contains(1);

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
        var result = dictionary.Keys.Contains(2);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Contains_CustomKeyType()
    {
        // Arrange
        var dictionary = new FasterDictionary<Tuple<int, int>, string>();
        var key1 = new Tuple<int, int>(1, 2);
        var key2 = new Tuple<int, int>(3, 4);
        dictionary[key1] = "value1";
        dictionary[key2] = "value2";

        // Act
        var result1 = dictionary.Keys.Contains(key1);
        var result2 = dictionary.Keys.Contains(new Tuple<int, int>(1, 2));
        var result3 = dictionary.Keys.Contains(new Tuple<int, int>(5, 6));

        // Assert
        Assert.True(result1);
        Assert.True(result2);
        Assert.False(result3);
    }
}
