// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Buffers;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Faster.Collections.Pooled;

/// <summary>
/// Provides an ArrayPool implementation meant to be used as the singleton returned from ArrayPool.Shared.
/// </summary>
/// <remarks>
/// Simplified implementation without multi-threading support.
/// </remarks>
internal sealed partial class SingleThreadedArrayPool<T> : ArrayPool<T>
{
    /// <summary>The shared instance of the pool.</summary>
    public new static SingleThreadedArrayPool<T> Shared { get; } = new();

    /// <summary>The number of buckets (array sizes) in the pool, one for each array length, starting from length 16.</summary>
    private const int NumBuckets = 27; // Utilities.SelectBucketIndex(1024 * 1024 * 1024 + 1)

    /// <summary>
    /// An array of buckets. The slots are lazily initialized to avoid creating
    /// lots of overhead for unused array sizes.
    /// </summary>
    private readonly Bucket?[] _buckets = new Bucket[NumBuckets];

    /// <summary>Allocate a new <see cref="Bucket"/> and try to store it into the <see cref="_buckets"/> array.</summary>
    private Bucket CreateBucket(int bucketIndex)
    {
        if (_buckets[bucketIndex] is null)
        {
            var inst = new Bucket();
            _buckets[bucketIndex] = inst;
            return inst;
        }
        return _buckets[bucketIndex]!;
    }

    public override T[] Rent(int minimumLength)
    {
        T[]? buffer;

        // Get the bucket number for the array length. The result may be out of range of buckets,
        // either for too large a value or for 0 and negative values.
        int bucketIndex = Utilities.SelectBucketIndex(minimumLength);

        // Try to get an array from the bucket.
        if ((uint)bucketIndex < (uint)_buckets.Length)
        {
            Bucket? b = _buckets[bucketIndex];
            if (b is not null)
            {
                buffer = Unsafe.As<T[]>(b.TryPop());
                if (buffer is not null)
                {
                    return buffer;
                }
            }

            // No buffer available.  Ensure the length we'll allocate matches that of a bucket
            // so we can later return it.
            minimumLength = Utilities.GetMaxSizeForBucket(bucketIndex);
        }
        else if (minimumLength == 0)
        {
            // We allow requesting zero-length arrays (even though pooling such an array isn't valuable)
            // as it's a valid length array, and we want the pool to be usable in general instead of using
            // `new`, even for computed lengths.
            return Array.Empty<T>();
        }
        else
        {
            ArgumentOutOfRangeException.ThrowIfNegative(minimumLength);
        }

        // For large arrays, we prefer to avoid the zero-initialization costs. However, as the resulting
        // arrays could end up containing arbitrary bit patterns, we only allow this for types for which
        // every possible bit pattern is valid.
        buffer = typeof(T).IsPrimitive && typeof(T) != typeof(bool) ?
            GC.AllocateUninitializedArray<T>(minimumLength) :
            new T[minimumLength];

        return buffer;
    }

    public override void Return(T[] array, bool clearArray = false)
    {
        ArgumentNullException.ThrowIfNull(array);

        // Determine with what bucket this array length is associated
        int bucketIndex = Utilities.SelectBucketIndex(array.Length);

        if ((uint)bucketIndex < (uint)_buckets.Length)
        {
            // Clear the array if the user requested it.
            if (clearArray)
            {
                Array.Clear(array);
            }

            // Check to see if the buffer is the correct size for this bucket.
            if (array.Length != Utilities.GetMaxSizeForBucket(bucketIndex))
            {
                throw new ArgumentException(nameof(array));
            }

            // Store the array into the bucket.
            Bucket bucketForArraySize = _buckets[bucketIndex] ?? CreateBucket(bucketIndex);
            bucketForArraySize.TryPush(array);
        }
    }

    public bool Trim()
    {
        int currentMilliseconds = Environment.TickCount;
        Utilities.MemoryPressure pressure = Utilities.GetMemoryPressure();

        // Trim each of the buckets.
        for (int i = 0; i < _buckets.Length; i++)
        {
            _buckets[i]?.Trim(currentMilliseconds, pressure);
        }

        return true;
    }
}

/// <summary>Provides a simple bucket for storing arrays.</summary>
internal sealed class Bucket
{
    /// <summary>The arrays in the bucket.</summary>
    private readonly Array?[] _arrays = new Array[SharedArrayPoolStatics.s_maxArraysPerBucket];
    /// <summary>Number of arrays stored in <see cref="_arrays"/>.</summary>
    private int _count;
    /// <summary>Timestamp set by Trim when it sees this as 0.</summary>
    private int _millisecondsTimestamp;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryPush(Array array)
    {
        Array?[] arrays = _arrays;
        int count = _count;
        if ((uint)count < (uint)arrays.Length)
        {
            if (count == 0)
            {
                // Reset the time stamp now that we're transitioning from empty to non-empty.
                // Trim will see this as 0 and initialize it to the current time when Trim is called.
                _millisecondsTimestamp = 0;
            }

            Unsafe.Add(ref MemoryMarshal.GetArrayDataReference(arrays), count) = array; // arrays[count] = array, but avoiding stelemref
            _count = count + 1;
            return true;
        }
        return false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Array? TryPop()
    {
        Array?[] arrays = _arrays;
        int count = _count - 1;
        if ((uint)count < (uint)arrays.Length)
        {
            Array? arr = arrays[count];
            arrays[count] = null;
            _count = count;
            return arr;
        }
        return null;
    }

    public void Trim(int currentMilliseconds, Utilities.MemoryPressure pressure)
    {
        const int TrimAfterMS = 60 * 1000;                                  // Trim after 60 seconds for low/moderate pressure
        const int HighTrimAfterMS = 10 * 1000;                              // Trim after 10 seconds for high pressure

        if (_count == 0)
        {
            return;
        }

        int trimMilliseconds = pressure == Utilities.MemoryPressure.High ? HighTrimAfterMS : TrimAfterMS;

        if (_count == 0)
        {
            return;
        }

        if (_millisecondsTimestamp == 0)
        {
            _millisecondsTimestamp = currentMilliseconds;
            return;
        }

        if ((currentMilliseconds - _millisecondsTimestamp) <= trimMilliseconds)
        {
            return;
        }

        // We've elapsed enough time since the first item went into the bucket.
        // Drop the top item(s) so they can be collected.

        int trimCount = pressure switch
        {
            Utilities.MemoryPressure.High => SharedArrayPoolStatics.s_maxArraysPerBucket,
            Utilities.MemoryPressure.Medium => 2,
            _ => 1,
        };

        while (_count > 0 && trimCount-- > 0)
        {
            Array? array = _arrays[--_count];
            Debug.Assert(array is not null, "No nulls should have been present in slots < _count.");
            _arrays[_count] = null;
        }

        _millisecondsTimestamp = _count > 0 ?
            _millisecondsTimestamp + (trimMilliseconds / 4) : // Give the remaining items a bit more time
            0;
    }
}

internal static class SharedArrayPoolStatics
{
    /// <summary>The maximum number of arrays per array size to store.</summary>
    internal static readonly int s_maxArraysPerBucket = GetMaxArraysPerBucket();

    /// <summary>Gets the maximum number of arrays of a given size allowed to be cached.</summary>
    /// <returns>Defaults to 32.</returns>
    private static int GetMaxArraysPerBucket()
    {
        return TryGetInt32EnvironmentVariable("DOTNET_SYSTEM_BUFFERS_SHAREDARRAYPOOL_MAXARRAYSPERBUCKET", out int result) && result > 0 ?
            result :
            32; // arbitrary limit
    }

    /// <summary>Look up an environment variable and try to parse it as an Int32.</summary>
    /// <remarks>This avoids using anything that might in turn recursively use the ArrayPool.</remarks>
    private static bool TryGetInt32EnvironmentVariable(string variable, out int result)
    {
        // Avoid globalization stack, as it might in turn be using ArrayPool.

        if (GetEnvironmentVariableCore_NoArrayPool(variable) is string envVar &&
            envVar.Length is > 0 and <= 32) // arbitrary limit that allows for some spaces around the maximum length of a non-negative Int32 (10 digits)
        {
            ReadOnlySpan<char> value = envVar.AsSpan().Trim(' ');
            if (!value.IsEmpty && value.Length <= 10)
            {
                long tempResult = 0;
                foreach (char c in value)
                {
                    uint digit = (uint)(c - '0');
                    if (digit > 9)
                    {
                        goto Fail;
                    }

                    tempResult = tempResult * 10 + digit;
                }

                if (tempResult is >= 0 and <= int.MaxValue)
                {
                    result = (int)tempResult;
                    return true;
                }
            }
        }

    Fail:
        result = 0;
        return false;
    }

    internal static string? GetEnvironmentVariableCore_NoArrayPool(string variable)
    {
        return Environment.GetEnvironmentVariable(variable);
    }
}
