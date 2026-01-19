// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;


namespace Faster.Collections.Pooled;

#pragma warning disable CS0649
internal sealed class RawArrayData
{
    public uint Length; // Array._numComponents padded to IntPtr
#if TARGET_64BIT
    public uint Padding;
#endif
    public byte Data;
}
#pragma warning restore CS0649

internal sealed class ArrayEnumerator : IEnumerator, ICloneable
{
    private readonly Array _array;
    private nint _index;

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    internal static uint NativeLength(Array array)
    {
        return Unsafe.As<RawArrayData>(array).Length;
    }

    internal ArrayEnumerator(Array array)
    {
        _array = array;
        _index = -1;
    }

    public object Clone()
    {
        return MemberwiseClone();
    }

    public bool MoveNext()
    {
        nint index = _index + 1;
        nuint length = NativeLength(_array);
        if ((nuint)index >= length)
        {
            _index = (nint)length;
            return false;
        }
        _index = index;
        return true;
    }

    public object? Current
    {
        get
        {
            nint index = _index;
            Array array = _array;

            if ((nuint)index >= NativeLength(array))
            {
                if (index < 0)
                {
                    throw new InvalidOperationException("Enumeration has not started. Call MoveNext.");
                }
                else
                {
                    throw new InvalidOperationException("Enumeration has already ended.");
                }
            }

            return array.GetValue(index);
        }
    }

    public void Reset()
    {
        _index = -1;
    }
}

internal abstract class SZGenericArrayEnumeratorBase : IDisposable
{
    protected int _index;
    protected readonly int _endIndex;

    protected SZGenericArrayEnumeratorBase(int endIndex)
    {
        _index = -1;
        _endIndex = endIndex;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool MoveNext()
    {
        int index = _index + 1;
        if ((uint)index < (uint)_endIndex)
        {
            _index = index;
            return true;
        }
        _index = _endIndex;
        return false;
    }

    public void Reset() => _index = -1;

    public void Dispose()
    {
    }
}

internal sealed class SZGenericArrayEnumerator<T> : SZGenericArrayEnumeratorBase, IEnumerator<T>
{
    private readonly T[]? _array;

    /// <summary>Provides an empty enumerator singleton.</summary>
    /// <remarks>
    /// If the consumer is using SZGenericArrayEnumerator elsewhere or is otherwise likely
    /// to be using T[] elsewhere, this singleton should be used.  Otherwise, GenericEmptyEnumerator's
    /// singleton should be used instead, as it doesn't reference T[] in order to reduce footprint.
    /// </remarks>
    internal static readonly SZGenericArrayEnumerator<T> Empty = new SZGenericArrayEnumerator<T>(null, 0);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal SZGenericArrayEnumerator(T[]? array, int endIndex)
        : base(endIndex)
    {
        Debug.Assert(array == null || endIndex == array.Length);
        _array = array;
    }

    public T Current
    {
        get
        {
            if ((uint)_index >= (uint)_endIndex)
                throw new InvalidOperationException("Enumeration has either not started or has already finished.");
            return _array![_index];
        }
    }

    object? IEnumerator.Current => Current;
}

internal abstract class GenericEmptyEnumeratorBase : IDisposable, IEnumerator
{
#pragma warning disable CA1822 // https://github.com/dotnet/roslyn-analyzers/issues/5911
    public bool MoveNext() => false;

    public object Current
    {
        get
        {
            throw new InvalidOperationException("Enumeration has either not started or has already finished.");
        }
    }

    public void Reset() { }

    public void Dispose() { }
#pragma warning restore CA1822
}

/// <summary>Provides an empty enumerator singleton.</summary>
/// <remarks>
/// If the consumer is using SZGenericArrayEnumerator elsewhere or is otherwise likely
/// to be using T[] elsewhere, SZGenericArrayEnumerator's singleton should be used.  Otherwise,
/// this singleton should be used, as it doesn't reference T[] in order to reduce footprint.
/// </remarks>
internal sealed class GenericEmptyEnumerator<T> : GenericEmptyEnumeratorBase, IEnumerator<T>
{
    public static readonly GenericEmptyEnumerator<T> Instance = new();

    private GenericEmptyEnumerator() { }

    public new T Current
    {
        get
        {
            throw new InvalidOperationException("Enumeration has either not started or has already finished.");
        }
    }
}