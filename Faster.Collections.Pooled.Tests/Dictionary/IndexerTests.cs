// Copyright (c) 2026, RickyYC and Contributors. All rights reserved.
// Distributed under the MIT Software License, Version 1.0.

namespace Faster.Collections.Pooled.Tests.Dictionary;

public class IndexerTests
{
    [Fact]
    public void Indexer_Get_ReturnsValue()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        dictionary[1] = "one";
        dictionary[2] = "two";

        // Act
        var value = dictionary[1];

        // Assert
        Assert.Equal("one", value);
    }

    [Fact]
    public void Indexer_Set_UpdatesValue()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        dictionary[1] = "one";

        // Act
        dictionary[1] = "updated";

        // Assert
        Assert.Equal("updated", dictionary[1]);
        Assert.Equal(1, dictionary.Count);
    }

    [Fact]
    public void Indexer_SetNonExistingKey_AddsNewItem()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();

        // Act
        dictionary[1] = "one";

        // Assert
        Assert.Equal(1, dictionary.Count);
        Assert.Equal("one", dictionary[1]);
    }

    [Fact]
    public void Indexer_ObjectKey_Get_ReturnsValue()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        dictionary[1] = "one";

        // Act
        var value = ((System.Collections.IDictionary)dictionary)[1];

        // Assert
        Assert.Equal("one", value);
    }

    [Fact]
    public void Indexer_ObjectKey_Set_UpdatesValue()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        dictionary[1] = "one";

        // Act
        ((System.Collections.IDictionary)dictionary)[1] = "updated";

        // Assert
        Assert.Equal("updated", dictionary[1]);
    }

    [Fact]
    public void Indexer_ObjectKey_SetNonExistingKey_AddsNewItem()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();

        // Act
        ((System.Collections.IDictionary)dictionary)[1] = "one";

        // Assert
        Assert.Equal(1, dictionary.Count);
        Assert.Equal("one", dictionary[1]);
    }
}
