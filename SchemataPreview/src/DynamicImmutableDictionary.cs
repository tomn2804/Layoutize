using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;

namespace SchemataPreview
{
	public partial class DynamicImmutableDictionary<T> : DynamicObject where T : IImmutableDictionary<string, object>
	{
		public DynamicImmutableDictionary(T dictionary)
		{
			Dictionary = dictionary;
		}

		public override bool TryGetMember(GetMemberBinder binder, out object? result)
		{
			return TryGetValue(binder.Name, out result);
		}

		public override bool TrySetMember(SetMemberBinder binder, object? value)
		{
			throw new ReadOnlyException();
		}

		protected T Dictionary { get; }
	}

	public partial class DynamicImmutableDictionary<T> : IImmutableDictionary<string, object>
	{
		public int Count => Dictionary.Count;
		public IEnumerable<string> Keys => Dictionary.Keys;
		public IEnumerable<object> Values => Dictionary.Values;
		public object this[string key] => Dictionary[key];

		public IImmutableDictionary<string, object> Add(string key, object value)
		{
			return Dictionary.Add(key, value);
		}

		public IImmutableDictionary<string, object> AddRange(IEnumerable<KeyValuePair<string, object>> pairs)
		{
			return Dictionary.AddRange(pairs);
		}

		public IImmutableDictionary<string, object> Clear()
		{
			return Dictionary.Clear();
		}

		public bool Contains(KeyValuePair<string, object> pair)
		{
			return Dictionary.Contains(pair);
		}

		public bool ContainsKey(string key)
		{
			return Dictionary.ContainsKey(key);
		}

		public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
		{
			return Dictionary.GetEnumerator();
		}

		public IImmutableDictionary<string, object> Remove(string key)
		{
			return Dictionary.Remove(key);
		}

		public IImmutableDictionary<string, object> RemoveRange(IEnumerable<string> keys)
		{
			return Dictionary.RemoveRange(keys);
		}

		public IImmutableDictionary<string, object> SetItem(string key, object value)
		{
			return Dictionary.SetItem(key, value);
		}

		public IImmutableDictionary<string, object> SetItems(IEnumerable<KeyValuePair<string, object>> items)
		{
			return Dictionary.SetItems(items);
		}

		public bool TryGetKey(string equalKey, out string actualKey)
		{
			return Dictionary.TryGetKey(equalKey, out actualKey);
		}

		public bool TryGetValue(string key, [MaybeNullWhen(false)] out object value)
		{
			return Dictionary.TryGetValue(key, out value);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)Dictionary).GetEnumerator();
		}
	}
}
