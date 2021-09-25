using System;
using System.Collections;
using System.Collections.Immutable;
using System.Management.Automation;

namespace SchemataPreview
{
	public abstract partial class Schema
	{
		public abstract Model Build();

		public abstract Model Build(string path);

		public abstract Model GetNewModel();

		public ImmutableSchema ToImmutable()
		{
			return new(Dictionary.ToImmutable());
		}

		protected Schema()
		{
		}

		protected Schema(Hashtable hashtable)
		{
			foreach (DictionaryEntry entry in hashtable)
			{
				if (entry.Value != null)
				{
					Add(entry.Key, entry.Value is PSObject obj ? obj.BaseObject : entry.Value);
				}
			}
		}

		protected ImmutableDictionary<object, object>.Builder Dictionary { get; } = ImmutableDictionary.CreateBuilder<object, object>();
	}

	public abstract partial class Schema : IDictionary
	{
		public int Count => ((ICollection)Dictionary).Count;
		public bool IsFixedSize => ((IDictionary)Dictionary).IsFixedSize;
		public bool IsReadOnly => ((IDictionary)Dictionary).IsReadOnly;
		public bool IsSynchronized => ((ICollection)Dictionary).IsSynchronized;
		public ICollection Keys => ((IDictionary)Dictionary).Keys;
		public object SyncRoot => ((ICollection)Dictionary).SyncRoot;
		public ICollection Values => ((IDictionary)Dictionary).Values;
		public object? this[object key] { get => ((IDictionary)Dictionary)[key]; set => ((IDictionary)Dictionary)[key] = value; }

		public void Add(object key, object? value)
		{
			((IDictionary)Dictionary).Add(key, value);
		}

		public void Clear()
		{
			((IDictionary)Dictionary).Clear();
		}

		public bool Contains(object key)
		{
			return ((IDictionary)Dictionary).Contains(key);
		}

		public void CopyTo(Array array, int index)
		{
			((ICollection)Dictionary).CopyTo(array, index);
		}

		public IDictionaryEnumerator GetEnumerator()
		{
			return ((IDictionary)Dictionary).GetEnumerator();
		}

		public void Remove(object key)
		{
			((IDictionary)Dictionary).Remove(key);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)Dictionary).GetEnumerator();
		}
	}

	public class Schema<T> : Schema where T : Model
	{
		public Schema()
		{
		}

		public Schema(Hashtable hashtable)
			: base(hashtable)
		{
		}

		public override T Build()
		{
			T model = GetNewModel();
			new Pipeline(model).Invoke(PipeOption.Mount);
			return model;
		}

		public override T Build(string path)
		{
			this["Path"] = path;
			return Build();
		}

		public override T GetNewModel()
		{
			return (T)(Activator.CreateInstance(typeof(T), ToImmutable()) ?? throw new MissingMethodException());
		}
	}
}
