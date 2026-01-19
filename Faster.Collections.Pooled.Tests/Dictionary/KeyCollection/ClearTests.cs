// Copyright (c) 2026, RickyYC and Contributors. All rights reserved.
// Distributed under the MIT Software License, Version 1.0.

namespace Faster.Collections.Pooled.Tests.Dictionary.Dictionary.KeyCollection;

public class ClearTests
{
    [Fact]
    public void Clear_AlwaysThrowsNotSupportedException()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        var keys = dictionary.Keys;

        // Act & Assert
        Assert.Throws<NotSupportedException>(() => keys.Clear());
    }

    [Fact]
    public void Clear_EmptyDictionary_ThrowsNotSupportedException()
    {
        // Arrange
        var dictionary = new FasterDictionary<string, int>();
        var keys = dictionary.Keys;

        // Act & Assert
        Assert.Throws<NotSupportedException>(() => keys.Clear());
    }

    [Fact]
    public void Clear_PopulatedDictionary_ThrowsNotSupportedException()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        dictionary[1] = "one";
        dictionary[2] = "two";
        var keys = dictionary.Keys;

        // Act & Assert
        Assert.Throws<NotSupportedException>(() => keys.Clear());
    }

    [Fact]
    public void Clear_ExceptionMessage_ContainsReadOnlyMessage()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        var keys = dictionary.Keys;

        // Act
        var exception = Assert.Throws<NotSupportedException>(() => keys.Clear());

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
        var keys = dictionary.Keys;

        // Act & Assert
        Assert.Throws<NotSupportedException>(() => keys.Clear());
        Assert.Equal(2, dictionary.Count);
        Assert.True(dictionary.ContainsKey(1));
        Assert.True(dictionary.ContainsKey(2));
    }
}
