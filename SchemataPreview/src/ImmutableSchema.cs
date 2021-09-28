using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Management.Automation;

namespace SchemataPreview
{
	public partial class ImmutableSchema
	{
		public ImmutableSchema(Schema schema)
		{
			ImmutableDictionary<object, object>.Builder dictionary = ImmutableDictionary.CreateBuilder<object, object>();
			foreach (DictionaryEntry entry in schema)
			{
				if (entry.Value is null)
				{
					throw new ArgumentNullException(entry.Key.ToString());
				}
				dictionary.Add(entry.Key, entry.Value is PSObject @object ? @object.BaseObject : entry.Value);
			}
			Dictionary = dictionary.ToImmutable();
		}

		protected ImmutableDictionary<object, object> Dictionary { get; }
	}

	public partial class ImmutableSchema : IImmutableDictionary<object, object>
	{
		public int Count => Dictionary.Count;
		public IEnumerable<object> Keys => Dictionary.Keys;
		public IEnumerable<object> Values => Dictionary.Values;
		public object this[object key] => Dictionary[key];

		public IImmutableDictionary<object, object> Add(object key, object value)
		{
			if (value is null)
			{
				throw new ArgumentNullException(nameof(value));
			}
			return Dictionary.Add(key, value is PSObject @object ? @object.BaseObject : value);
		}

		public IImmutableDictionary<object, object> AddRange(IEnumerable<KeyValuePair<object, object>> pairs)
		{
			ImmutableDictionary<object, object>.Builder result = Dictionary.ToBuilder();
			foreach (KeyValuePair<object, object> pair in pairs)
			{
				if (pair.Value is null)
				{
					throw new ArgumentNullException(pair.Key.ToString());
				}
				result.Add(pair.Key, pair.Value is PSObject @object ? @object.BaseObject : pair.Value);
			}
			return result.ToImmutable();
		}

		public IImmutableDictionary<object, object> Clear()
		{
			return Dictionary.Clear();
		}

		public bool Contains(KeyValuePair<object, object> pair)
		{
			return Dictionary.Contains(pair);
		}

		public bool ContainsKey(object key)
		{
			return Dictionary.ContainsKey(key);
		}

		public IEnumerator<KeyValuePair<object, object>> GetEnumerator()
		{
			return Dictionary.GetEnumerator();
		}

		public IImmutableDictionary<object, object> Remove(object key)
		{
			return Dictionary.Remove(key);
		}

		public IImmutableDictionary<object, object> RemoveRange(IEnumerable<object> keys)
		{
			return Dictionary.RemoveRange(keys);
		}

		public IImmutableDictionary<object, object> SetItem(object key, object value)
		{
			if (value is null)
			{
				throw new ArgumentNullException(nameof(value));
			}
			return Dictionary.SetItem(key, value is PSObject @object ? @object.BaseObject : value);
		}

		public IImmutableDictionary<object, object> SetItems(IEnumerable<KeyValuePair<object, object>> items)
		{
			ImmutableDictionary<object, object>.Builder result = Dictionary.ToBuilder();
			foreach (KeyValuePair<object, object> item in items)
			{
				if (item.Value is null)
				{
					throw new ArgumentNullException(item.Key.ToString());
				}
				result[item.Key] = item.Value is PSObject @object ? @object.BaseObject : item.Value;
			}
			return result.ToImmutable();
		}

		public bool TryGetKey(object equalKey, out object actualKey)
		{
			return Dictionary.TryGetKey(equalKey, out actualKey);
		}

		public bool TryGetValue(object key, [MaybeNullWhen(false)] out object value)
		{
			return Dictionary.TryGetValue(key, out value);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return Dictionary.GetEnumerator();
		}
	}
}
