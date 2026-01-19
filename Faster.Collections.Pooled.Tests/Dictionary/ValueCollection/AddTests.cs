// Copyright (c) 2026, RickyYC and Contributors. All rights reserved.
// Distributed under the MIT Software License, Version 1.0.

namespace Faster.Collections.Pooled.Tests.Dictionary.ValueCollection;

public class AddTests
{
    [Fact]
    public void Add_AlwaysThrowsNotSupportedException()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        var values = dictionary.Values;

        // Act & Assert
        Assert.Throws<NotSupportedException>(() => values.Add("test"));
    }

    [Fact]
    public void Add_ToEmptyDictionary_ThrowsNotSupportedException()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        var values = dictionary.Values;

        // Act & Assert
        Assert.Throws<NotSupportedException>(() => values.Add("test"));
    }

    [Fact]
    public void Add_ToPopulatedDictionary_ThrowsNotSupportedException()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        dictionary[1] = "one";
        var values = dictionary.Values;

        // Act & Assert
        Assert.Throws<NotSupportedException>(() => values.Add("two"));
    }

    [Fact]
    public void Add_ExceptionMessage_ContainsReadOnlyMessage()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        var values = dictionary.Values;

        // Act
        var exception = Assert.Throws<NotSupportedException>(() => values.Add("test"));

        // Assert
        Assert.Contains("read-only", exception.Message, StringComparison.OrdinalIgnoreCase);
    }
}
