// Copyright (c) 2026, RickyYC and Contributors. All rights reserved.
// Distributed under the MIT Software License, Version 1.0.

using System.Collections;

namespace Faster.Collections.Pooled.Tests.Dictionary.Dictionary.KeyCollection;

public class IsSynchronizedTests
{
    [Fact]
    public void IsSynchronized_AlwaysReturnsFalse()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();

        // Act
        var isSynchronized = ((ICollection)dictionary.Keys).IsSynchronized;

        // Assert
        Assert.False(isSynchronized);
    }

    [Fact]
    public void IsSynchronized_EmptyDictionary_ReturnsFalse()
    {
        // Arrange
        var dictionary = new FasterDictionary<string, int>();

        // Act
        var isSynchronized = ((ICollection)dictionary.Keys).IsSynchronized;

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
        var isSynchronized = ((ICollection)dictionary.Keys).IsSynchronized;

        // Assert
        Assert.False(isSynchronized);
    }
}
