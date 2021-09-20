using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;

namespace SchemataPreview
{
	public partial class DynamicDictionary : DynamicObject
	{
		public DynamicDictionary()
		{
		}

		public DynamicDictionary(Hashtable hashtable)
		{
			foreach (DictionaryEntry entry in hashtable)
			{
				if (entry.Value != null)
				{
					Add((string)entry.Key, entry.Value);
				}
			}
		}

		public override bool TryGetMember(GetMemberBinder binder, out object? result)
		{
			return Dictionary.TryGetValue(binder.Name, out result);
		}

		public override bool TrySetMember(SetMemberBinder binder, object? value)
		{
			if (value == null)
			{
				throw new ArgumentNullException();
			}
			if (!Dictionary.TryAdd(binder.Name, value))
			{
				Dictionary[binder.Name] = value;
			}
			return true;
		}

		protected Dictionary<string, object> Dictionary = new(StringComparer.InvariantCultureIgnoreCase);
	}

	public partial class DynamicDictionary : IDictionary<string, object>
	{
		public int Count => ((ICollection<KeyValuePair<string, object>>)Dictionary).Count;
		public bool IsReadOnly => ((ICollection<KeyValuePair<string, object>>)Dictionary).IsReadOnly;
		public ICollection<string> Keys => ((IDictionary<string, object>)Dictionary).Keys;
		public ICollection<object> Values => ((IDictionary<string, object>)Dictionary).Values;
		public object this[string key] { get => ((IDictionary<string, object>)Dictionary)[key]; set => ((IDictionary<string, object>)Dictionary)[key] = value; }

		public void Add(string key, object value)
		{
			((IDictionary<string, object>)Dictionary).Add(key, value);
		}

		public void Add(KeyValuePair<string, object> item)
		{
			((ICollection<KeyValuePair<string, object>>)Dictionary).Add(item);
		}

		public void Clear()
		{
			((ICollection<KeyValuePair<string, object>>)Dictionary).Clear();
		}

		public bool Contains(KeyValuePair<string, object> item)
		{
			return ((ICollection<KeyValuePair<string, object>>)Dictionary).Contains(item);
		}

		public bool ContainsKey(string key)
		{
			return ((IDictionary<string, object>)Dictionary).ContainsKey(key);
		}

		public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
		{
			((ICollection<KeyValuePair<string, object>>)Dictionary).CopyTo(array, arrayIndex);
		}

		public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
		{
			return ((IEnumerable<KeyValuePair<string, object>>)Dictionary).GetEnumerator();
		}

		public bool Remove(string key)
		{
			return ((IDictionary<string, object>)Dictionary).Remove(key);
		}

		public bool Remove(KeyValuePair<string, object> item)
		{
			return ((ICollection<KeyValuePair<string, object>>)Dictionary).Remove(item);
		}

		public bool TryGetValue(string key, [MaybeNullWhen(false)] out object value)
		{
			return ((IDictionary<string, object>)Dictionary).TryGetValue(key, out value);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)Dictionary).GetEnumerator();
		}
	}
}
