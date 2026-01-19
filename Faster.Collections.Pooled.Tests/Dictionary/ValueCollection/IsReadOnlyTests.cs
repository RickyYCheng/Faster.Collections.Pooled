// Copyright (c) 2026, RickyYC and Contributors. All rights reserved.
// Distributed under the MIT Software License, Version 1.0.

namespace Faster.Collections.Pooled.Tests.Dictionary.ValueCollection;

public class IsReadOnlyTests
{
    [Fact]
    public void IsReadOnly_AlwaysReturnsTrue()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();

        // Act
        var isReadOnly = dictionary.Values.IsReadOnly;

        // Assert
        Assert.True(isReadOnly);
    }

    [Fact]
    public void IsReadOnly_EmptyDictionary_ReturnsTrue()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();

        // Act
        var isReadOnly = dictionary.Values.IsReadOnly;

        // Assert
        Assert.True(isReadOnly);
    }

    [Fact]
    public void IsReadOnly_PopulatedDictionary_ReturnsTrue()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        dictionary[1] = "one";
        dictionary[2] = "two";

        // Act
        var isReadOnly = dictionary.Values.IsReadOnly;

        // Assert
        Assert.True(isReadOnly);
    }
}
