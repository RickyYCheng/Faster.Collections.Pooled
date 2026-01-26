// Copyright (c) 2026, RickyYC and Contributors. All rights reserved.
// Distributed under the MIT Software License, Version 1.0.

using System.Collections;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Faster.Collections.Pooled.Hashing;

namespace Faster.Collections.Pooled;

[DebuggerTypeProxy(typeof(IDictionaryDebugView<,>))]
[DebuggerDisplay("Count = {Count}")]
// [Serializable]
public partial class FasterDictionary<TKey, TValue> :
    PooledBlitzMap<TKey, TValue, DefaultHasher.Generic<TKey>>,
    ICollection<KeyValuePair<TKey, TValue>>,
    IDictionary<TKey, TValue>,
    IEnumerable<KeyValuePair<TKey, TValue>>,
    IReadOnlyCollection<KeyValuePair<TKey, TValue>>,
    IReadOnlyDictionary<TKey, TValue>,
    IDictionary
    where TKey : notnull
{
    public FasterDictionary() : base(2, 0.8) { }

    public FasterDictionary(int length) : base(length, 0.8) { }

    public FasterDictionary(int length, double loadfactor) : base(length, loadfactor) { }

    public FasterDictionary(Dictionary<TKey, TValue> source)
        : this(source.Count)
    {
        foreach (var kvp in source)
        {
            Add(kvp.Key, kvp.Value);
        }
    }

    public FasterDictionary(IDictionary<TKey, TValue> source)
        : this(source.Count)
    {
        foreach (var kvp in source)
        {
            Add(kvp.Key, kvp.Value);
        }
    }

    public FasterDictionary(IReadOnlyDictionary<TKey, TValue> source)
        : this(source.Count)
    {
        foreach (var kvp in source)
        {
            Add(kvp.Key, kvp.Value);
        }
    }

    public FasterDictionary(IEnumerable<KeyValuePair<TKey, TValue>> source)
        : this()
    {
        foreach (var kvp in source)
        {
            Add(kvp.Key, kvp.Value);
        }
    }

    public FasterDictionary(Span<KeyValuePair<TKey, TValue>> source)
        : this(source.Length)
    {
        for (int i = 0; i < source.Length; i++)
        {
            var kvp = source[i];
            Add(kvp.Key, kvp.Value);
        }
    }

    public FasterDictionary(ReadOnlySpan<KeyValuePair<TKey, TValue>> source)
        : this(source.Length)
    {
        for (int i = 0; i < source.Length; i++)
        {
            var kvp = source[i];
            Add(kvp.Key, kvp.Value);
        }
    }

    public FasterDictionary(Memory<KeyValuePair<TKey, TValue>> source) : this(source.Span) { }
    public FasterDictionary(ReadOnlyMemory<KeyValuePair<TKey, TValue>> source) : this(source.Span) { }
    public FasterDictionary(KeyValuePair<TKey, TValue>[] source) : this((ReadOnlySpan<KeyValuePair<TKey, TValue>>)source) { }

    // Tuple-based constructors
    public FasterDictionary(IEnumerable<(TKey Key, TValue Value)> source)
        : this()
    {
        foreach (var (Key, Value) in source)
        {
            Add(Key, Value);
        }
    }

    public FasterDictionary(Span<(TKey Key, TValue Value)> source)
        : this(source.Length)
    {
        for (int i = 0; i < source.Length; i++)
        {
            var (Key, Value) = source[i];
            Add(Key, Value);
        }
    }

    public FasterDictionary(ReadOnlySpan<(TKey Key, TValue Value)> source)
        : this(source.Length)
    {
        for (int i = 0; i < source.Length; i++)
        {
            var (Key, Value) = source[i];
            Add(Key, Value);
        }
    }

    public FasterDictionary(Memory<(TKey Key, TValue Value)> source) : this(source.Span) { }
    public FasterDictionary(ReadOnlyMemory<(TKey Key, TValue Value)> source) : this(source.Span) { }
    public FasterDictionary((TKey Key, TValue Value)[] source) : this((ReadOnlySpan<(TKey Key, TValue Value)>)source) { }

    private KeyCollection? _keys;
    private ValueCollection? _values;

    object? IDictionary.this[object key]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        get => base[(TKey)key];
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        set => base[(TKey)key] = (TValue)value!;
    }

    public bool IsFixedSize
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        get => false;
    }

    public bool IsReadOnly
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        get => false;
    }

    bool IDictionary.IsReadOnly
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        get => false;
    }

    ICollection IDictionary.Keys
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        get => Keys;
    }

    ICollection IDictionary.Values
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        get => Values;
    }

    public bool IsSynchronized
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        get => false;
    }

    public object SyncRoot
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        get => this;
    }

    IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        get => Keys;
    }

    ICollection<TKey> IDictionary<TKey, TValue>.Keys
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        get => Keys;
    }

    IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        get => Values;
    }

    ICollection<TValue> IDictionary<TKey, TValue>.Values
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        get => Values;
    }

    public KeyCollection Keys
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        get => _keys ??= new KeyCollection(this);
    }

    public ValueCollection Values
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        get => _values ??= new ValueCollection(this);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    void IDictionary.Add(object key, object? value) =>
        base.Insert((TKey)key, (TValue?)value!);

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public void Add(TKey key, TValue value) =>
        base.Insert(key, value);

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public void Add(KeyValuePair<TKey, TValue> item) =>
        base.Insert(item.Key, item.Value);

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    bool IDictionary.Contains(object key) =>
        base.Contains((TKey)key);

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item) =>
        base.Contains(item.Key) && EqualityComparer<TValue>.Default.Equals(base[item.Key], item.Value);

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool ContainsKey(TKey key) =>
        base.Contains(key);

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public void CopyTo(Array array, int index)
    {
        ArgumentNullException.ThrowIfNull(array);
        ArgumentOutOfRangeException.ThrowIfNegative(index);
        if (array.Length - index < _count)
            throw new ArgumentException("Insufficient space in the target array.");

        for (int i = 0; i < _count; i++)
        {
            ref readonly var entry = ref _entries[i];
            array.SetValue(new DictionaryEntry(entry.Key!, entry.Value), index + i);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
        ArgumentNullException.ThrowIfNull(array);
        ArgumentOutOfRangeException.ThrowIfNegative(arrayIndex);
        if (array.Length - arrayIndex < _count)
            throw new ArgumentException("Insufficient space in the target array.");

        for (int i = 0; i < _count; i++)
        {
            ref readonly var entry = ref _entries[i];
            array[arrayIndex + i] = new KeyValuePair<TKey, TValue>(entry.Key, entry.Value);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    void IDictionary.Remove(object key) => 
        base.Remove((TKey)key);

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
    {
        // TODO: improve performance
        // Low performance since we should do extra equality check (for pair.Value)
        // There's no comparer in BlitzMap currently
        // And double lookup here is not ideal
        return ((ICollection<KeyValuePair<TKey, TValue>>)this).Contains(item) && base.Remove(item.Key);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value) =>
        base.Get(key, out value);

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    IDictionaryEnumerator IDictionary.GetEnumerator() => 
        new Enumerator(this);

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator() => 
        new Enumerator(this);

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    IEnumerator IEnumerable.GetEnumerator() => 
        new Enumerator(this);

    public partial struct Enumerator : IEnumerator<KeyValuePair<TKey, TValue>>, IDictionaryEnumerator
    {
        private readonly FasterDictionary<TKey, TValue> _dictionary;
        private readonly int _count;
        private int _index;

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public Enumerator(FasterDictionary<TKey, TValue> dictionary)
        {
            _dictionary = dictionary;
            _count = (int)_dictionary._count;
            _index = -1;
        }

        public readonly KeyValuePair<TKey, TValue> Current
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get
            {
                ref readonly var entry = ref _dictionary._entries[_index];
                return new KeyValuePair<TKey, TValue>(entry.Key, entry.Value);
            }
        }

        readonly DictionaryEntry IDictionaryEnumerator.Entry
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get
            {
                ref readonly var entry = ref _dictionary._entries[_index];
                return new DictionaryEntry(entry.Key!, entry.Value);
            }
        }

        public readonly object Key
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get => _dictionary._entries[_index].Key!;
        }

        public readonly object? Value
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get => _dictionary._entries[_index].Value;
        }

        readonly object IEnumerator.Current
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get => Current;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public bool MoveNext()
        {
            int next = _index + 1;
            if (next < _count)
            {
                _index = next;
                return true;
            }
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public void Reset()
        {
            _index = -1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public void Dispose() { }
    }

    public sealed partial class KeyCollection :
        ICollection<TKey>,
        IReadOnlyCollection<TKey>,
        ICollection
    {
        private readonly FasterDictionary<TKey, TValue> _dictionary;

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public KeyCollection(FasterDictionary<TKey, TValue> dictionary)
        {
            _dictionary = dictionary;
        }

        public int Count
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get => _dictionary.Count;
        }

        public bool IsReadOnly
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get => true;
        }

        public bool IsSynchronized
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get => false;
        }

        public object SyncRoot
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get => _dictionary;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public void Add(TKey item)
            => throw new NotSupportedException("Collection is read-only.");

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public void Clear()
            => throw new NotSupportedException("Collection is read-only.");

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public bool Contains(TKey item)
            => _dictionary.ContainsKey(item);

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public void CopyTo(TKey[] array, int arrayIndex)
        {
            ArgumentNullException.ThrowIfNull(array);
            ArgumentOutOfRangeException.ThrowIfNegative(arrayIndex);
            if (array.Length - arrayIndex < _dictionary._count)
                throw new ArgumentException("Insufficient space in the target array.");

            for (int i = 0; i < _dictionary._count; i++)
            {
                array[arrayIndex + i] = _dictionary._entries[i].Key;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public void CopyTo(Array array, int index)
        {
            ArgumentNullException.ThrowIfNull(array);
            ArgumentOutOfRangeException.ThrowIfNegative(index);
            if (array.Length - index < _dictionary._count)
                throw new ArgumentException("Insufficient space in the target array.");

            for (int i = 0; i < _dictionary._count; i++)
            {
                array.SetValue(_dictionary._entries[i].Key!, index + i);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public bool Remove(TKey item)
            => throw new NotSupportedException("Collection is read-only.");

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        IEnumerator<TKey> IEnumerable<TKey>.GetEnumerator()
            => new KeyEnumerator(_dictionary);

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public SpanEnumerator GetEnumerator()
            => new SpanEnumerator(_dictionary);

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        IEnumerator IEnumerable.GetEnumerator()
            => new KeyEnumerator(_dictionary);

        public partial struct KeyEnumerator : IEnumerator<TKey>
        {
            private readonly FasterDictionary<TKey, TValue> _dictionary;
            private readonly int _count;
            private int _index;

            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            public KeyEnumerator(FasterDictionary<TKey, TValue> dictionary)
            {
                _dictionary = dictionary;
                _count = (int)_dictionary._count;
                _index = -1;
            }

            public readonly TKey Current
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
                get => _dictionary._entries[_index].Key;
            }

            readonly object IEnumerator.Current
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
                get => Current!;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            public bool MoveNext()
            {
                int next = _index + 1;
                if (next < _count)
                {
                    _index = next;
                    return true;
                }
                return false;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            public void Reset()
            {
                _index = -1;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            public void Dispose() { }
        }

        public ref struct SpanEnumerator
        {
            private readonly FasterDictionary<TKey, TValue> _dictionary;
            private readonly int _count;
            private int _index;

            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            public SpanEnumerator(FasterDictionary<TKey, TValue> dictionary)
            {
                _dictionary = dictionary;
                _count = (int)_dictionary._count;
                _index = -1;
            }

            public readonly TKey Current
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
                get => _dictionary._entries[_index].Key;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            public bool MoveNext()
            {
                int next = _index + 1;
                if (next < _count)
                {
                    _index = next;
                    return true;
                }
                return false;
            }
        }
    }

    public sealed partial class ValueCollection :
        ICollection<TValue>,
        IReadOnlyCollection<TValue>,
        ICollection
    {
        private readonly FasterDictionary<TKey, TValue> _dictionary;

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public ValueCollection(FasterDictionary<TKey, TValue> dictionary)
        {
            _dictionary = dictionary;
        }

        public int Count
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get => _dictionary.Count;
        }

        public bool IsReadOnly
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get => true;
        }

        public bool IsSynchronized
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get => false;
        }

        public object SyncRoot
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get => _dictionary;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public void Add(TValue item)
            => throw new NotSupportedException("Collection is read-only.");

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public void Clear()
            => throw new NotSupportedException("Collection is read-only.");

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public bool Contains(TValue item)
        {
            var comparer = EqualityComparer<TValue>.Default;
            for (int i = 0; i < _dictionary._count; i++)
            {
                if (comparer.Equals(_dictionary._entries[i].Value, item))
                    return true;
            }
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public void CopyTo(TValue[] array, int arrayIndex)
        {
            ArgumentNullException.ThrowIfNull(array);
            ArgumentOutOfRangeException.ThrowIfNegative(arrayIndex);
            if (array.Length - arrayIndex < _dictionary._count)
                throw new ArgumentException("Insufficient space in the target array.");

            for (int i = 0; i < _dictionary._count; i++)
            {
                array[arrayIndex + i] = _dictionary._entries[i].Value;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public void CopyTo(Array array, int index)
        {
            ArgumentNullException.ThrowIfNull(array);
            ArgumentOutOfRangeException.ThrowIfNegative(index);
            if (array.Length - index < _dictionary._count)
                throw new ArgumentException("Insufficient space in the target array.");

            for (int i = 0; i < _dictionary._count; i++)
            {
                array.SetValue(_dictionary._entries[i].Value, index + i);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public bool Remove(TValue item)
            => throw new NotSupportedException("Collection is read-only.");

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator()
            => new ValueEnumerator(_dictionary);

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public SpanEnumerator GetEnumerator()
            => new(_dictionary);

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        IEnumerator IEnumerable.GetEnumerator()
            => new ValueEnumerator(_dictionary);

        public partial struct ValueEnumerator : IEnumerator<TValue>
        {
            private readonly FasterDictionary<TKey, TValue> _dictionary;
            private readonly int _count;
            private int _index;

            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            public ValueEnumerator(FasterDictionary<TKey, TValue> dictionary)
            {
                _dictionary = dictionary;
                _count = (int)_dictionary._count;
                _index = -1;
            }

            public readonly TValue Current
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
                get => _dictionary._entries[_index].Value;
            }

            readonly object? IEnumerator.Current
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
                get => Current;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            public bool MoveNext()
            {
                int next = _index + 1;
                if (next < _count)
                {
                    _index = next;
                    return true;
                }
                return false;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            public void Reset()
            {
                _index = -1;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            public void Dispose() { }
        }

        public ref struct SpanEnumerator
        {
            private readonly FasterDictionary<TKey, TValue> _dictionary;
            private readonly int _count;
            private int _index;

            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            public SpanEnumerator(FasterDictionary<TKey, TValue> dictionary)
            {
                _dictionary = dictionary;
                _count = (int)_dictionary._count;
                _index = -1;
            }

            public readonly TValue Current
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
                get => _dictionary._entries[_index].Value;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            public bool MoveNext()
            {
                int next = _index + 1;
                if (next < _count)
                {
                    _index = next;
                    return true;
                }
                return false;
            }
        }
    }
}
