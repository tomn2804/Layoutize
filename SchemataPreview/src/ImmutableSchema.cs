using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace SchemataPreview
{
	public partial class ImmutableSchema
	{
		public ImmutableSchema(ImmutableDictionary<object, object> dictionary)
		{
			Dictionary = dictionary;
		}

		protected ImmutableDictionary<object, object> Dictionary { get; }
	}

	public partial class ImmutableSchema : IImmutableDictionary<object, object>
	{
		public int Count => ((IReadOnlyCollection<KeyValuePair<object, object>>)Dictionary).Count;
		public IEnumerable<object> Keys => ((IReadOnlyDictionary<object, object>)Dictionary).Keys;
		public IEnumerable<object> Values => ((IReadOnlyDictionary<object, object>)Dictionary).Values;
		public object this[object key] => ((IReadOnlyDictionary<object, object>)Dictionary)[key];

		public IImmutableDictionary<object, object> Add(object key, object value)
		{
			return ((IImmutableDictionary<object, object>)Dictionary).Add(key, value);
		}

		public IImmutableDictionary<object, object> AddRange(IEnumerable<KeyValuePair<object, object>> pairs)
		{
			return ((IImmutableDictionary<object, object>)Dictionary).AddRange(pairs);
		}

		public IImmutableDictionary<object, object> Clear()
		{
			return ((IImmutableDictionary<object, object>)Dictionary).Clear();
		}

		public bool Contains(KeyValuePair<object, object> pair)
		{
			return ((IImmutableDictionary<object, object>)Dictionary).Contains(pair);
		}

		public bool ContainsKey(object key)
		{
			return ((IReadOnlyDictionary<object, object>)Dictionary).ContainsKey(key);
		}

		public IEnumerator<KeyValuePair<object, object>> GetEnumerator()
		{
			return ((IEnumerable<KeyValuePair<object, object>>)Dictionary).GetEnumerator();
		}

		public IImmutableDictionary<object, object> Remove(object key)
		{
			return ((IImmutableDictionary<object, object>)Dictionary).Remove(key);
		}

		public IImmutableDictionary<object, object> RemoveRange(IEnumerable<object> keys)
		{
			return ((IImmutableDictionary<object, object>)Dictionary).RemoveRange(keys);
		}

		public IImmutableDictionary<object, object> SetItem(object key, object value)
		{
			return ((IImmutableDictionary<object, object>)Dictionary).SetItem(key, value);
		}

		public IImmutableDictionary<object, object> SetItems(IEnumerable<KeyValuePair<object, object>> items)
		{
			return ((IImmutableDictionary<object, object>)Dictionary).SetItems(items);
		}

		public bool TryGetKey(object equalKey, out object actualKey)
		{
			return ((IImmutableDictionary<object, object>)Dictionary).TryGetKey(equalKey, out actualKey);
		}

		public bool TryGetValue(object key, [MaybeNullWhen(false)] out object value)
		{
			return ((IReadOnlyDictionary<object, object>)Dictionary).TryGetValue(key, out value);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)Dictionary).GetEnumerator();
		}
	}
}
