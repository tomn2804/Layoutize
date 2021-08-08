using System;
using System.Collections;
using System.Data;
using System.Dynamic;

namespace SchemataPreview
{
	public partial class ReadOnlySchema : DynamicObject
	{
		internal ReadOnlySchema(Schema schema)
			: base()
		{
			Schema = schema;
		}

		protected internal Schema Schema { get; init; }

		public override bool TryGetMember(GetMemberBinder binder, out object result)
		{
			return Schema.TryGetMember(binder, out result);
		}

		public override bool TrySetMember(SetMemberBinder binder, object value)
		{
			throw new ReadOnlyException();
		}
	}

	public partial class ReadOnlySchema : IDictionary
	{
		public object this[object key] { get => Schema[key]; set => throw new ReadOnlyException(); }

		public bool IsFixedSize => Schema.IsFixedSize;

		public bool IsReadOnly => true;

		public ICollection Keys => Schema.Keys;

		public ICollection Values => Schema.Values;

		public int Count => Schema.Count;

		public bool IsSynchronized => Schema.IsSynchronized;

		public object SyncRoot => Schema.SyncRoot;

		public void Add(object key, object value)
		{
			throw new ReadOnlyException();
		}

		public void Clear()
		{
			throw new ReadOnlyException();
		}

		public bool Contains(object key)
		{
			return Schema.Contains(key);
		}

		public void CopyTo(Array array, int index)
		{
			Schema.CopyTo(array, index);
		}

		public IDictionaryEnumerator GetEnumerator()
		{
			return Schema.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return Schema.GetEnumerator();
		}

		public void Remove(object key)
		{
			throw new ReadOnlyException();
		}
	}
}
