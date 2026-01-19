// Copyright (c) 2026, RickyYC and Contributors. All rights reserved.
// Distributed under the MIT Software License, Version 1.0.

namespace Faster.Collections.Pooled.Tests.Dictionary;

public class RemoveTests
{
    [Fact]
    public void Remove_ObjectKey_RemovesItem()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        dictionary[1] = "one";
        dictionary[2] = "two";

        // Act
        ((System.Collections.IDictionary)dictionary).Remove(1);

        // Assert
        Assert.Equal(1, dictionary.Count);
        Assert.False(dictionary.ContainsKey(1));
        Assert.True(dictionary.ContainsKey(2));
    }

    [Fact]
    public void Remove_ObjectKey_NonExistingKey_DoesNothing()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        dictionary[1] = "one";
        var initialCount = dictionary.Count;

        // Act
        ((System.Collections.IDictionary)dictionary).Remove(99);

        // Assert
        Assert.Equal(initialCount, dictionary.Count);
        Assert.True(dictionary.ContainsKey(1));
    }

    [Fact]
    public void Remove_KeyValuePair_RemovesItem()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        dictionary[1] = "one";
        dictionary[2] = "two";
        var item = new System.Collections.Generic.KeyValuePair<int, string>(1, "one");

        // Act
        var result = ((System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<int, string>>)dictionary).Remove(item);

        // Assert
        Assert.True(result);
        Assert.Equal(1, dictionary.Count);
        Assert.False(dictionary.ContainsKey(1));
    }

    [Fact]
    public void Remove_KeyValuePair_WrongValue_DoesNotRemove()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        dictionary[1] = "one";
        var item = new System.Collections.Generic.KeyValuePair<int, string>(1, "wrong");

        // Act
        var result = ((System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<int, string>>)dictionary).Remove(item);

        // Assert
        Assert.False(result);
        Assert.Equal(1, dictionary.Count);
        Assert.True(dictionary.ContainsKey(1));
    }

    [Fact]
    public void Remove_KeyValuePair_NonExistingKey_ReturnsFalse()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        dictionary[1] = "one";
        var item = new System.Collections.Generic.KeyValuePair<int, string>(99, "value");

        // Act
        var result = ((System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<int, string>>)dictionary).Remove(item);

        // Assert
        Assert.False(result);
        Assert.Equal(1, dictionary.Count);
    }
}
