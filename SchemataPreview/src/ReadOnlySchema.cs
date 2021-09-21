using System;
using System.Collections;
using System.Data;
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

	public partial class ReadOnlySchema : IDictionary
	{
		public bool IsReadOnly => true;
		public int Count => ((ICollection)Schema).Count;
		public bool IsFixedSize => ((IDictionary)Schema).IsFixedSize;
		public bool IsSynchronized => ((ICollection)Schema).IsSynchronized;
		public ICollection Keys => ((IDictionary)Schema).Keys;
		public object SyncRoot => ((ICollection)Schema).SyncRoot;
		public ICollection Values => ((IDictionary)Schema).Values;
		public object? this[object key] { get => ((IDictionary)Schema)[key]; set => ((IDictionary)Schema)[key] = value; }

		public void Add(object key, object? value)
		{
			((IDictionary)Schema).Add(key, value);
		}

		public void Clear()
		{
			((IDictionary)Schema).Clear();
		}

		public bool Contains(object key)
		{
			return ((IDictionary)Schema).Contains(key);
		}

		public void CopyTo(Array array, int index)
		{
			((ICollection)Schema).CopyTo(array, index);
		}

		public IDictionaryEnumerator GetEnumerator()
		{
			return ((IDictionary)Schema).GetEnumerator();
		}

		public void Remove(object key)
		{
			((IDictionary)Schema).Remove(key);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)Schema).GetEnumerator();
		}
	}
}
