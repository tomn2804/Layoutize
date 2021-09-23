using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;

namespace SchemataPreview
{
	public partial class DynamicDictionary<T> : DynamicObject where T : IDictionary<string, object>
	{
		public DynamicDictionary(T dictionary)
		{
			Dictionary = dictionary;
		}

		public override bool TryGetMember(GetMemberBinder binder, out object? result)
		{
			return TryGetValue(binder.Name, out result);
		}

		public override bool TrySetMember(SetMemberBinder binder, object? value)
		{
			this[binder.Name] = value ?? throw new ArgumentNullException(nameof(value));
			return true;
		}

		protected T Dictionary { get; }
	}

	public partial class DynamicDictionary<T> : IDictionary<string, object>
	{
		public int Count => Dictionary.Count;
		public bool IsReadOnly => Dictionary.IsReadOnly;
		public ICollection<string> Keys => Dictionary.Keys;
		public ICollection<object> Values => Dictionary.Values;
		public object this[string key] { get => Dictionary[key]; set => Dictionary[key] = value; }

		public void Add(string key, object value)
		{
			Dictionary.Add(key, value);
		}

		public void Add(KeyValuePair<string, object> item)
		{
			Dictionary.Add(item);
		}

		public void Clear()
		{
			Dictionary.Clear();
		}

		public bool Contains(KeyValuePair<string, object> item)
		{
			return Dictionary.Contains(item);
		}

		public bool ContainsKey(string key)
		{
			return Dictionary.ContainsKey(key);
		}

		public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
		{
			Dictionary.CopyTo(array, arrayIndex);
		}

		public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
		{
			return Dictionary.GetEnumerator();
		}

		public bool Remove(string key)
		{
			return Dictionary.Remove(key);
		}

		public bool Remove(KeyValuePair<string, object> item)
		{
			return Dictionary.Remove(item);
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
