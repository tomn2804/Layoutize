using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;

namespace SchemataPreview
{
	public partial class ReadOnlySchema : DynamicObject
	{
		public ReadOnlySchema(Schema schema)
		{
			Schema = schema;
		}

		public override bool TryGetMember(GetMemberBinder binder, out object? result)
		{
			return Schema.TryGetMember(binder, out result);
		}

		public override bool TrySetMember(SetMemberBinder binder, object? value)
		{
			throw new ReadOnlyException();
		}

		protected Schema Schema { get; init; }
	}

	public partial class ReadOnlySchema : IDictionary<string, object>
	{
		public int Count => ((ICollection<KeyValuePair<string, object>>)Schema).Count;
		public bool IsReadOnly => ((ICollection<KeyValuePair<string, object>>)Schema).IsReadOnly;
		public ICollection<string> Keys => ((IDictionary<string, object>)Schema).Keys;
		public ICollection<object> Values => ((IDictionary<string, object>)Schema).Values;
		public object this[string key] { get => ((IDictionary<string, object>)Schema)[key]; set => ((IDictionary<string, object>)Schema)[key] = value; }

		public void Add(string key, object value)
		{
			((IDictionary<string, object>)Schema).Add(key, value);
		}

		public void Add(KeyValuePair<string, object> item)
		{
			((ICollection<KeyValuePair<string, object>>)Schema).Add(item);
		}

		public void Clear()
		{
			((ICollection<KeyValuePair<string, object>>)Schema).Clear();
		}

		public bool Contains(KeyValuePair<string, object> item)
		{
			return ((ICollection<KeyValuePair<string, object>>)Schema).Contains(item);
		}

		public bool ContainsKey(string key)
		{
			return ((IDictionary<string, object>)Schema).ContainsKey(key);
		}

		public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
		{
			((ICollection<KeyValuePair<string, object>>)Schema).CopyTo(array, arrayIndex);
		}

		public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
		{
			return ((IEnumerable<KeyValuePair<string, object>>)Schema).GetEnumerator();
		}

		public bool Remove(string key)
		{
			return ((IDictionary<string, object>)Schema).Remove(key);
		}

		public bool Remove(KeyValuePair<string, object> item)
		{
			return ((ICollection<KeyValuePair<string, object>>)Schema).Remove(item);
		}

		public bool TryGetValue(string key, [MaybeNullWhen(false)] out object value)
		{
			return ((IDictionary<string, object>)Schema).TryGetValue(key, out value);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)Schema).GetEnumerator();
		}
	}
}
