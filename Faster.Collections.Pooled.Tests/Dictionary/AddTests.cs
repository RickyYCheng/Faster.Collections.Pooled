// Copyright (c) 2026, RickyYC and Contributors. All rights reserved.
// Distributed under the MIT Software License, Version 1.0.

namespace Faster.Collections.Pooled.Tests.Dictionary;

public class AddTests
{
    [Fact]
    public void Add_KeyValue_AddsItem()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();

        // Act
        dictionary.Add(1, "one");

        // Assert
        Assert.Equal(1, dictionary.Count);
        Assert.True(dictionary.ContainsKey(1));
        Assert.Equal("one", dictionary[1]);
    }

    [Fact]
    public void Add_MultipleItems_AddsAllItems()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();

        // Act
        dictionary.Add(1, "one");
        dictionary.Add(2, "two");
        dictionary.Add(3, "three");

        // Assert
        Assert.Equal(3, dictionary.Count);
    }

    [Fact]
    public void Add_KeyValuePair_AddsItem()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        var item = new System.Collections.Generic.KeyValuePair<int, string>(1, "one");

        // Act
        dictionary.Add(item);

        // Assert
        Assert.Equal(1, dictionary.Count);
        Assert.True(dictionary.ContainsKey(1));
        Assert.Equal("one", dictionary[1]);
    }

    [Fact]
    public void Add_ObjectKeyAndValue_AddsItem()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();

        // Act
        ((System.Collections.IDictionary)dictionary).Add(1, "one");

        // Assert
        Assert.Equal(1, dictionary.Count);
        Assert.True(dictionary.ContainsKey(1));
        Assert.Equal("one", dictionary[1]);
    }
}
