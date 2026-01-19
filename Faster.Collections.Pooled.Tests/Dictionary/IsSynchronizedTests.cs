// Copyright (c) 2026, RickyYC and Contributors. All rights reserved.
// Distributed under the MIT Software License, Version 1.0.

namespace Faster.Collections.Pooled.Tests.Dictionary;

public class IsSynchronizedTests
{
    [Fact]
    public void IsSynchronized_AlwaysReturnsFalse()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();

        // Act
        var isSynchronized = ((System.Collections.IDictionary)dictionary).IsSynchronized;

        // Assert
        Assert.False(isSynchronized);
    }

    [Fact]
    public void IsSynchronized_EmptyDictionary_ReturnsFalse()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();

        // Act
        var isSynchronized = ((System.Collections.IDictionary)dictionary).IsSynchronized;

        // Assert
        Assert.False(isSynchronized);
    }

    [Fact]
    public void IsSynchronized_PopulatedDictionary_ReturnsFalse()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        dictionary[1] = "one";
        dictionary[2] = "two";

        // Act
        var isSynchronized = ((System.Collections.IDictionary)dictionary).IsSynchronized;

        // Assert
        Assert.False(isSynchronized);
    }
}
