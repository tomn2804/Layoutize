using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace SchemataPreview
{
    public partial class ImmutableDefinition
    {
        public partial class Builder
        {
            public Builder()
            {
                Dictionary = ImmutableDictionary.CreateBuilder<object, object>();
            }

            public Builder(IDictionary dictionary)
            {
                Dictionary = ImmutableDictionary.CreateBuilder<object, object>();
                foreach (DictionaryEntry entry in dictionary)
                {
                    Dictionary.Add(entry.Key, entry.Value ?? throw new ArgumentNullException(nameof(entry.Value)));
                }
            }

            public Builder(ImmutableDefinition definition)
            {
                Dictionary = definition.Dictionary.ToBuilder();
            }

            public ImmutableDefinition ToImmutable()
            {
                return new(Dictionary.ToImmutable());
            }

            protected ImmutableDictionary<object, object>.Builder Dictionary { get; }
            private IDictionary<object, object> IDictionary => Dictionary;
        }

        public partial class Builder : IDictionary<object, object>
        {
            public ICollection<object> Keys => IDictionary.Keys;
            public ICollection<object> Values => IDictionary.Values;
            public int Count => IDictionary.Count;
            public bool IsReadOnly => IDictionary.IsReadOnly;

            public object this[object key]
            {
                get => IDictionary[key];
                set
                {
                    if (value is null)
                    {
                        throw new ArgumentNullException(null, nameof(value));
                    }
                    switch (key)
                    {
                        case DefinitionOperator.Spread:
                            DefinitionOperation.Spread(IDictionary, value is IDictionary dictionary ? dictionary : throw new ArgumentException(null, nameof(value)));
                            break;

                        default:
                            IDictionary[key] = value;
                            break;
                    }
                }
            }

            public void Add(object key, object value)
            {
                if (value is null)
                {
                    throw new ArgumentNullException(nameof(value));
                }
                switch (key)
                {
                    case DefinitionOperator.Spread:
                        DefinitionOperation.Spread(IDictionary, value is IDictionary dictionary ? dictionary : throw new ArgumentException(null, nameof(value)));
                        break;

                    default:
                        IDictionary.Add(key, value);
                        break;
                }
            }

            public void Add(KeyValuePair<object, object> item)
            {
                if (item.Value is null)
                {
                    throw new ArgumentNullException(nameof(item.Value));
                }
                switch (item.Key)
                {
                    case DefinitionOperator.Spread:
                        DefinitionOperation.Spread(IDictionary, item.Value is IDictionary dictionary ? dictionary : throw new ArgumentException(null, nameof(item.Value)));
                        break;

                    default:
                        IDictionary.Add(item);
                        break;
                }
            }

            public void Clear()
            {
                IDictionary.Clear();
            }

            public bool Contains(KeyValuePair<object, object> item)
            {
                return IDictionary.Contains(item);
            }

            public bool ContainsKey(object key)
            {
                return IDictionary.ContainsKey(key);
            }

            public void CopyTo(KeyValuePair<object, object>[] array, int arrayIndex)
            {
                IDictionary.CopyTo(array, arrayIndex);
            }

            public IEnumerator<KeyValuePair<object, object>> GetEnumerator()
            {
                return IDictionary.GetEnumerator();
            }

            public bool Remove(object key)
            {
                return IDictionary.Remove(key);
            }

            public bool Remove(KeyValuePair<object, object> item)
            {
                return ((ICollection<KeyValuePair<object, object>>)IDictionary).Remove(item);
            }

            public bool TryGetValue(object key, [MaybeNullWhen(false)] out object value)
            {
                return IDictionary.TryGetValue(key, out value);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return ((IEnumerable)IDictionary).GetEnumerator();
            }
        }
    }
}
