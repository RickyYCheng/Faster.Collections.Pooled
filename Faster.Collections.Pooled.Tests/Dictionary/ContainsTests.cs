// Copyright (c) 2026, RickyYC and Contributors. All rights reserved.
// Distributed under the MIT Software License, Version 1.0.

namespace Faster.Collections.Pooled.Tests.Dictionary;

public class ContainsTests
{
    [Fact]
    public void Contains_ObjectKey_ReturnsTrueForExistingKey()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        dictionary[1] = "one";
        dictionary[2] = "two";

        // Act
        var result = ((System.Collections.IDictionary)dictionary).Contains(1);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Contains_ObjectKey_ReturnsFalseForNonExistingKey()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        dictionary[1] = "one";

        // Act
        var result = ((System.Collections.IDictionary)dictionary).Contains(99);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Contains_KeyValuePair_ReturnsTrueForExistingPair()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        dictionary[1] = "one";
        dictionary[2] = "two";
        var item = new System.Collections.Generic.KeyValuePair<int, string>(1, "one");

        // Act
        var result = ((System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<int, string>>)dictionary).Contains(item);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Contains_KeyValuePair_ReturnsFalseForNonExistingPair()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        dictionary[1] = "one";
        var item = new System.Collections.Generic.KeyValuePair<int, string>(1, "two");

        // Act
        var result = ((System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<int, string>>)dictionary).Contains(item);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Contains_KeyValuePair_ReturnsFalseForNonExistingKey()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        dictionary[1] = "one";
        var item = new System.Collections.Generic.KeyValuePair<int, string>(99, "value");

        // Act
        var result = ((System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<int, string>>)dictionary).Contains(item);

        // Assert
        Assert.False(result);
    }
}
