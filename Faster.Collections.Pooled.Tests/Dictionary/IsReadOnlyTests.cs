// Copyright (c) 2026, RickyYC and Contributors. All rights reserved.
// Distributed under the MIT Software License, Version 1.0.

namespace Faster.Collections.Pooled.Tests.Dictionary;

public class IsReadOnlyTests
{
    [Fact]
    public void IsReadOnly_AlwaysReturnsFalse()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();

        // Act
        var isReadOnly = ((System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<int, string>>)dictionary).IsReadOnly;

        // Assert
        Assert.False(isReadOnly);
    }

    [Fact]
    public void IsReadOnly_EmptyDictionary_ReturnsFalse()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();

        // Act
        var isReadOnly = ((System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<int, string>>)dictionary).IsReadOnly;

        // Assert
        Assert.False(isReadOnly);
    }

    [Fact]
    public void IsReadOnly_PopulatedDictionary_ReturnsFalse()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        dictionary[1] = "one";
        dictionary[2] = "two";

        // Act
        var isReadOnly = ((System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<int, string>>)dictionary).IsReadOnly;

        // Assert
        Assert.False(isReadOnly);
    }

    [Fact]
    public void IsReadOnly_IDictionaryInterface_ReturnsFalse()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();

        // Act
        var isReadOnly = ((System.Collections.IDictionary)dictionary).IsReadOnly;

        // Assert
        Assert.False(isReadOnly);
    }
}
