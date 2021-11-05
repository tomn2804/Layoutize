using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace Schemata
{
    public partial class ImmutableOutline
    {
        public partial class Builder
        {
            public Builder()
            {
                Dictionary = ImmutableDictionary.CreateBuilder<object, object?>();
            }

            public Builder(IDictionary dictionary)
            {
                Dictionary = ImmutableDictionary.CreateBuilder<object, object?>();
                foreach (DictionaryEntry entry in dictionary)
                {
                    Dictionary.Add(entry.Key, entry.Value);
                }
            }

            public Builder(ImmutableOutline outline)
            {
                Dictionary = outline.Dictionary.ToBuilder();
            }

            public ImmutableOutline ToImmutable()
            {
                return new(Dictionary.ToImmutable());
            }

            protected ImmutableDictionary<object, object?>.Builder Dictionary { get; }
            private IDictionary IDictionary => Dictionary;
        }

        public partial class Builder : IDictionary
        {
            public int Count => IDictionary.Count;
            public bool IsFixedSize => IDictionary.IsFixedSize;
            public bool IsReadOnly => IDictionary.IsReadOnly;
            public bool IsSynchronized => IDictionary.IsSynchronized;
            public ICollection Keys => IDictionary.Keys;
            public object SyncRoot => IDictionary.SyncRoot;
            public ICollection Values => IDictionary.Values;
            public object? this[object key] { get => IDictionary[key]; set => IDictionary[key] = value; }

            public void Add(object key, object? value)
            {
                IDictionary.Add(key, value);
            }

            public void Clear()
            {
                IDictionary.Clear();
            }

            public bool Contains(object key)
            {
                return IDictionary.Contains(key);
            }

            public void CopyTo(Array array, int index)
            {
                IDictionary.CopyTo(array, index);
            }

            public IDictionaryEnumerator GetEnumerator()
            {
                return IDictionary.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return IDictionary.GetEnumerator();
            }

            public void Remove(object key)
            {
                IDictionary.Remove(key);
            }
        }
    }
}
