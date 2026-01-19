// Copyright (c) 2026, RickyYC and Contributors. All rights reserved.
// Distributed under the MIT Software License, Version 1.0.

namespace Faster.Collections.Pooled.Tests.Dictionary.ValueCollection;

public class ClearTests
{
    [Fact]
    public void Clear_AlwaysThrowsNotSupportedException()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        var values = dictionary.Values;

        // Act & Assert
        Assert.Throws<NotSupportedException>(() => values.Clear());
    }

    [Fact]
    public void Clear_EmptyDictionary_ThrowsNotSupportedException()
    {
        // Arrange
        var dictionary = new FasterDictionary<string, int>();
        var values = dictionary.Values;

        // Act & Assert
        Assert.Throws<NotSupportedException>(() => values.Clear());
    }

    [Fact]
    public void Clear_PopulatedDictionary_ThrowsNotSupportedException()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        dictionary[1] = "one";
        dictionary[2] = "two";
        var values = dictionary.Values;

        // Act & Assert
        Assert.Throws<NotSupportedException>(() => values.Clear());
    }

    [Fact]
    public void Clear_ExceptionMessage_ContainsReadOnlyMessage()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        var values = dictionary.Values;

        // Act
        var exception = Assert.Throws<NotSupportedException>(() => values.Clear());

        // Assert
        Assert.Contains("read-only", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Clear_DictionaryRemainsIntact()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        dictionary[1] = "one";
        dictionary[2] = "two";
        var values = dictionary.Values;

        // Act & Assert
        Assert.Throws<NotSupportedException>(() => values.Clear());
        Assert.Equal(2, dictionary.Count);
        Assert.True(dictionary.ContainsKey(1));
        Assert.True(dictionary.ContainsKey(2));
    }
}
