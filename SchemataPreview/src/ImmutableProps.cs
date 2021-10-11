using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace SchemataPreview
{
    public partial class ImmutableProps
    {
        public static implicit operator ImmutableProps(Builder builder)
        {
            return builder.ToImmutable();
        }

        public Builder ToBuilder()
        {
            return new(this);
        }

        protected ImmutableDictionary<object, object> Dictionary { get; }

        private ImmutableProps(ImmutableDictionary<object, object> dictionary)
        {
            Dictionary = dictionary;
        }

        private IImmutableDictionary<object, object> IDictionary => Dictionary;
    }

    public partial class ImmutableProps : IImmutableDictionary<object, object>
    {
        public int Count => IDictionary.Count;
        public IEnumerable<object> Keys => IDictionary.Keys;
        public IEnumerable<object> Values => IDictionary.Values;
        public object this[object key] => IDictionary[key];

        public IImmutableDictionary<object, object> Add(object key, object value)
        {
            Builder result = ToBuilder();
            result.Add(key, value);
            return result.ToImmutable();
        }

        public IImmutableDictionary<object, object> AddRange(IEnumerable<KeyValuePair<object, object>> pairs)
        {
            Builder result = ToBuilder();
            foreach (KeyValuePair<object, object> pair in pairs)
            {
                result.Add(pair);
            }
            return result.ToImmutable();
        }

        public IImmutableDictionary<object, object> Clear()
        {
            return IDictionary.Clear();
        }

        public bool Contains(KeyValuePair<object, object> pair)
        {
            return IDictionary.Contains(pair);
        }

        public bool ContainsKey(object key)
        {
            return IDictionary.ContainsKey(key);
        }

        public IEnumerator<KeyValuePair<object, object>> GetEnumerator()
        {
            return IDictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return IDictionary.GetEnumerator();
        }

        public IImmutableDictionary<object, object> Remove(object key)
        {
            return IDictionary.Remove(key);
        }

        public IImmutableDictionary<object, object> RemoveRange(IEnumerable<object> keys)
        {
            return IDictionary.RemoveRange(keys);
        }

        public IImmutableDictionary<object, object> SetItem(object key, object value)
        {
            Builder result = ToBuilder();
            result[key] = value;
            return result.ToImmutable();
        }

        public IImmutableDictionary<object, object> SetItems(IEnumerable<KeyValuePair<object, object>> items)
        {
            Builder result = ToBuilder();
            foreach (KeyValuePair<object, object> item in items)
            {
                result[item.Key] = item.Value;
            }
            return result.ToImmutable();
        }

        public bool TryGetKey(object equalKey, out object actualKey)
        {
            return IDictionary.TryGetKey(equalKey, out actualKey);
        }

        public bool TryGetValue(object key, [MaybeNullWhen(false)] out object value)
        {
            return IDictionary.TryGetValue(key, out value);
        }
    }
}
