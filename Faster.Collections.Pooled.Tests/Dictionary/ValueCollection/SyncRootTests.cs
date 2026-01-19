// Copyright (c) 2026, RickyYC and Contributors. All rights reserved.
// Distributed under the MIT Software License, Version 1.0.

using System.Collections;

namespace Faster.Collections.Pooled.Tests.Dictionary.ValueCollection;

public class SyncRootTests
{
    [Fact]
    public void SyncRoot_ReturnsDictionaryInstance()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();

        // Act
        var syncRoot = ((ICollection)dictionary.Values).SyncRoot;

        // Assert
        Assert.Same(dictionary, syncRoot);
    }

    [Fact]
    public void SyncRoot_EmptyDictionary_ReturnsDictionaryInstance()
    {
        // Arrange
        var dictionary = new FasterDictionary<string, int>();

        // Act
        var syncRoot = ((ICollection)dictionary.Values).SyncRoot;

        // Assert
        Assert.Same(dictionary, syncRoot);
    }

    [Fact]
    public void SyncRoot_PopulatedDictionary_ReturnsDictionaryInstance()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();
        dictionary[1] = "one";
        dictionary[2] = "two";

        // Act
        var syncRoot = ((ICollection)dictionary.Values).SyncRoot;

        // Assert
        Assert.Same(dictionary, syncRoot);
    }

    [Fact]
    public void SyncRoot_MultipleCalls_ReturnsSameInstance()
    {
        // Arrange
        var dictionary = new FasterDictionary<int, string>();

        // Act
        var syncRoot1 = ((ICollection)dictionary.Values).SyncRoot;
        var syncRoot2 = ((ICollection)dictionary.Values).SyncRoot;

        // Assert
        Assert.Same(syncRoot1, syncRoot2);
    }
}
