// Copyright (c) 2026, RickyYC and Contributors. All rights reserved.
// Distributed under the MIT Software License, Version 1.0.

namespace Faster.Collections.Pooled.Tests.Dictionary;

public class IsFixedSizeTests
{
    [Fact]
    public void IsFixedSize_AlwaysReturnsFalse()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();

        // Act
        var isFixedSize = ((System.Collections.IDictionary)dictionary).IsFixedSize;

        // Assert
        Assert.False(isFixedSize);
    }

    [Fact]
    public void IsFixedSize_EmptyDictionary_ReturnsFalse()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();

        // Act
        var isFixedSize = ((System.Collections.IDictionary)dictionary).IsFixedSize;

        // Assert
        Assert.False(isFixedSize);
    }

    [Fact]
    public void IsFixedSize_PopulatedDictionary_ReturnsFalse()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        dictionary[1] = "one";
        dictionary[2] = "two";

        // Act
        var isFixedSize = ((System.Collections.IDictionary)dictionary).IsFixedSize;

        // Assert
        Assert.False(isFixedSize);
    }
}
