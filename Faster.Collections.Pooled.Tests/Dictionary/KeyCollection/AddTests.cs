// Copyright (c) 2026, RickyYC and Contributors. All rights reserved.
// Distributed under the MIT Software License, Version 1.0.

namespace Faster.Collections.Pooled.Tests.Dictionary.Dictionary.KeyCollection;

public class AddTests
{
    [Fact]
    public void Add_AlwaysThrowsNotSupportedException()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        var keys = dictionary.Keys;

        // Act & Assert
        Assert.Throws<NotSupportedException>(() => keys.Add(1));
    }

    [Fact]
    public void Add_ToEmptyDictionary_ThrowsNotSupportedException()
    {
        // Arrange
        var dictionary = new FasterDictionary<string, int>();
        var keys = dictionary.Keys;

        // Act & Assert
        Assert.Throws<NotSupportedException>(() => keys.Add("test"));
    }

    [Fact]
    public void Add_ToPopulatedDictionary_ThrowsNotSupportedException()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        dictionary[1] = "one";
        var keys = dictionary.Keys;

        // Act & Assert
        Assert.Throws<NotSupportedException>(() => keys.Add(2));
    }

    [Fact]
    public void Add_ExceptionMessage_ContainsReadOnlyMessage()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        var keys = dictionary.Keys;

        // Act
        var exception = Assert.Throws<NotSupportedException>(() => keys.Add(1));

        // Assert
        Assert.Contains("read-only", exception.Message, StringComparison.OrdinalIgnoreCase);
    }
}
