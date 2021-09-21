using System;
using System.Collections;
using System.Dynamic;

namespace SchemataPreview
{
	public partial class DynamicHashtable : DynamicObject
	{
		public DynamicHashtable()
		{
		}

		public DynamicHashtable(Hashtable hashtable)
		{
			Hashtable = hashtable;
		}

		public override bool TryGetMember(GetMemberBinder binder, out object? result)
		{
			result = this[binder.Name];
			return result != null;
		}

		public override bool TrySetMember(SetMemberBinder binder, object? value)
		{
			if (!Contains(binder.Name))
			{
				return false;
			}
			this[binder.Name] = value;
			return true;
		}

		protected Hashtable Hashtable { get; } = new(StringComparer.InvariantCultureIgnoreCase);
	}

	public partial class DynamicHashtable : IDictionary
	{
		public int Count => ((ICollection)Hashtable).Count;
		public bool IsFixedSize => ((IDictionary)Hashtable).IsFixedSize;
		public bool IsReadOnly => ((IDictionary)Hashtable).IsReadOnly;
		public bool IsSynchronized => ((ICollection)Hashtable).IsSynchronized;
		public ICollection Keys => ((IDictionary)Hashtable).Keys;
		public object SyncRoot => ((ICollection)Hashtable).SyncRoot;
		public ICollection Values => ((IDictionary)Hashtable).Values;
		public object? this[object key] { get => ((IDictionary)Hashtable)[key]; set => ((IDictionary)Hashtable)[key] = value; }

		public void Add(object key, object? value)
		{
			((IDictionary)Hashtable).Add(key, value);
		}

		public void Clear()
		{
			((IDictionary)Hashtable).Clear();
		}

		public bool Contains(object key)
		{
			return ((IDictionary)Hashtable).Contains(key);
		}

		public void CopyTo(Array array, int index)
		{
			((ICollection)Hashtable).CopyTo(array, index);
		}

		public IDictionaryEnumerator GetEnumerator()
		{
			return ((IDictionary)Hashtable).GetEnumerator();
		}

		public void Remove(object key)
		{
			((IDictionary)Hashtable).Remove(key);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)Hashtable).GetEnumerator();
		}
	}
}
