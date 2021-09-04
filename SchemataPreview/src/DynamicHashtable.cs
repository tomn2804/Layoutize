﻿using System;
using System.Collections;
using System.Dynamic;
using System.Management.Automation;

namespace SchemataPreview
{
	public partial class DynamicHashtable : DynamicObject
	{
		public DynamicHashtable()
		{
		}

		public DynamicHashtable(Hashtable hashtable)
		{
			foreach (DictionaryEntry entry in hashtable)
			{
				Hashtable.Add(entry.Key, entry.Value is PSObject obj ? obj.BaseObject : entry.Value);
			}
		}

		public override bool TryGetMember(GetMemberBinder binder, out object? result)
		{
			result = this[binder.Name];
			if (result == null)
			{
				CurrentShell.WriteWarning(new NullReferenceException($"Property {binder.Name} is uninstalized."));
				return false;
			}
			return true;
		}

		public override bool TrySetMember(SetMemberBinder binder, object? value)
		{
			if (!Contains(binder.Name))
			{
				CurrentShell.WriteWarning(new NullReferenceException($"Property {binder.Name} is uninstalized."));
				return false;
			}
			this[binder.Name] = value;
			return true;
		}

		protected Hashtable Hashtable { get; } = new(StringComparer.InvariantCultureIgnoreCase);
	}

	public partial class DynamicHashtable : IDictionary
	{
		public int Count => Hashtable.Count;

		public bool IsFixedSize => Hashtable.IsFixedSize;
		public bool IsReadOnly => Hashtable.IsReadOnly;
		public bool IsSynchronized => Hashtable.IsSynchronized;

		public ICollection Keys => Hashtable.Keys;
		public object SyncRoot => Hashtable.SyncRoot;
		public ICollection Values => Hashtable.Values;
		public object? this[object key] { get => Hashtable[key]; set => Hashtable[key] = value; }

		public void Add(object key, object? value)
		{
			Hashtable.Add(key, value);
		}

		public void Clear()
		{
			Hashtable.Clear();
		}

		public bool Contains(object key)
		{
			return Hashtable.Contains(key);
		}

		public void CopyTo(Array array, int index)
		{
			Hashtable.CopyTo(array, index);
		}

		public IDictionaryEnumerator GetEnumerator()
		{
			return Hashtable.GetEnumerator();
		}

		public void Remove(object key)
		{
			Hashtable.Remove(key);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return Hashtable.GetEnumerator();
		}
	}
}
