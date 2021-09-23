using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;

namespace SchemataPreview
{
	public partial class DynamicDictionary<TDictionary, TValue> : DynamicObject where TDictionary : IDictionary<string, TValue>
	{
		public DynamicDictionary(TDictionary dictionary)
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
			this[binder.Name] = (TValue?)value ?? throw new ArgumentNullException(nameof(value));
			return true;
		}

		protected TDictionary Dictionary { get; }
	}

	public partial class DynamicDictionary<TDictionary, TValue> : IDictionary<string, TValue>
	{
		public int Count => Dictionary.Count;
		public bool IsReadOnly => Dictionary.IsReadOnly;
		public ICollection<string> Keys => Dictionary.Keys;
		public ICollection<TValue> Values => Dictionary.Values;
		public TValue this[string key] { get => Dictionary[key]; set => Dictionary[key] = value; }

		public void Add(string key, TValue value)
		{
			Dictionary.Add(key, value);
		}

		public void Add(KeyValuePair<string, TValue> item)
		{
			Dictionary.Add(item);
		}

		public void Clear()
		{
			Dictionary.Clear();
		}

		public bool Contains(KeyValuePair<string, TValue> item)
		{
			return Dictionary.Contains(item);
		}

		public bool ContainsKey(string key)
		{
			return Dictionary.ContainsKey(key);
		}

		public void CopyTo(KeyValuePair<string, TValue>[] array, int arrayIndex)
		{
			Dictionary.CopyTo(array, arrayIndex);
		}

		public IEnumerator<KeyValuePair<string, TValue>> GetEnumerator()
		{
			return Dictionary.GetEnumerator();
		}

		public bool Remove(string key)
		{
			return Dictionary.Remove(key);
		}

		public bool Remove(KeyValuePair<string, TValue> item)
		{
			return Dictionary.Remove(item);
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
