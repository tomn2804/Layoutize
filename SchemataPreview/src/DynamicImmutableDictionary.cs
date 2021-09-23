using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;

namespace SchemataPreview
{
	public partial class DynamicImmutableDictionary<TDictionary, TValue> : DynamicObject where TDictionary : IImmutableDictionary<string, TValue>
	{
		public DynamicImmutableDictionary(TDictionary dictionary)
		{
			Dictionary = dictionary;
		}

		public override bool TryGetMember(GetMemberBinder binder, out object? result)
		{
			bool hasResult = TryGetValue(binder.Name, out TValue? value);
			result = value;
			return hasResult;
		}

		public override bool TrySetMember(SetMemberBinder binder, object? value)
		{
			throw new ReadOnlyException();
		}

		protected TDictionary Dictionary { get; }
	}

	public partial class DynamicImmutableDictionary<TDictionary, TValue> : IImmutableDictionary<string, TValue>
	{
		public int Count => Dictionary.Count;
		public IEnumerable<string> Keys => Dictionary.Keys;
		public IEnumerable<TValue> Values => Dictionary.Values;
		public TValue this[string key] => Dictionary[key];

		public IImmutableDictionary<string, TValue> Add(string key, TValue value)
		{
			return Dictionary.Add(key, value);
		}

		public IImmutableDictionary<string, TValue> AddRange(IEnumerable<KeyValuePair<string, TValue>> pairs)
		{
			return Dictionary.AddRange(pairs);
		}

		public IImmutableDictionary<string, TValue> Clear()
		{
			return Dictionary.Clear();
		}

		public bool Contains(KeyValuePair<string, TValue> pair)
		{
			return Dictionary.Contains(pair);
		}

		public bool ContainsKey(string key)
		{
			return Dictionary.ContainsKey(key);
		}

		public IEnumerator<KeyValuePair<string, TValue>> GetEnumerator()
		{
			return Dictionary.GetEnumerator();
		}

		public IImmutableDictionary<string, TValue> Remove(string key)
		{
			return Dictionary.Remove(key);
		}

		public IImmutableDictionary<string, TValue> RemoveRange(IEnumerable<string> keys)
		{
			return Dictionary.RemoveRange(keys);
		}

		public IImmutableDictionary<string, TValue> SetItem(string key, TValue value)
		{
			return Dictionary.SetItem(key, value);
		}

		public IImmutableDictionary<string, TValue> SetItems(IEnumerable<KeyValuePair<string, TValue>> items)
		{
			return Dictionary.SetItems(items);
		}

		public bool TryGetKey(string equalKey, out string actualKey)
		{
			return Dictionary.TryGetKey(equalKey, out actualKey);
		}

		public bool TryGetValue(string key, [MaybeNullWhen(false)] out TValue value)
		{
			return Dictionary.TryGetValue(key, out value);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)Dictionary).GetEnumerator();
		}
	}
}
