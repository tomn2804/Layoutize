using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace SchemataPreview
{
	public partial class PipeAssembly
	{
		protected Dictionary<object, Pipe> KeyToPipe { get; } = new();
	}

	public partial class PipeAssembly : IDictionary<object, Pipe>
	{
		public int Count => ((ICollection<KeyValuePair<object, Pipe>>)KeyToPipe).Count;
		public bool IsReadOnly => ((ICollection<KeyValuePair<object, Pipe>>)KeyToPipe).IsReadOnly;
		public ICollection<object> Keys => ((IDictionary<object, Pipe>)KeyToPipe).Keys;
		public ICollection<Pipe> Values => ((IDictionary<object, Pipe>)KeyToPipe).Values;
		public Pipe this[object key] { get => ((IDictionary<object, Pipe>)KeyToPipe)[key]; set => ((IDictionary<object, Pipe>)KeyToPipe)[key] = value; }

		public void Add(object key, Pipe value)
		{
			((IDictionary<object, Pipe>)KeyToPipe).Add(key, value);
		}

		public void Add(KeyValuePair<object, Pipe> item)
		{
			((ICollection<KeyValuePair<object, Pipe>>)KeyToPipe).Add(item);
		}

		public void Clear()
		{
			((ICollection<KeyValuePair<object, Pipe>>)KeyToPipe).Clear();
		}

		public bool Contains(KeyValuePair<object, Pipe> item)
		{
			return ((ICollection<KeyValuePair<object, Pipe>>)KeyToPipe).Contains(item);
		}

		public bool ContainsKey(object key)
		{
			return ((IDictionary<object, Pipe>)KeyToPipe).ContainsKey(key);
		}

		public void CopyTo(KeyValuePair<object, Pipe>[] array, int arrayIndex)
		{
			((ICollection<KeyValuePair<object, Pipe>>)KeyToPipe).CopyTo(array, arrayIndex);
		}

		public IEnumerator<KeyValuePair<object, Pipe>> GetEnumerator()
		{
			return ((IEnumerable<KeyValuePair<object, Pipe>>)KeyToPipe).GetEnumerator();
		}

		public bool Remove(object key)
		{
			return ((IDictionary<object, Pipe>)KeyToPipe).Remove(key);
		}

		public bool Remove(KeyValuePair<object, Pipe> item)
		{
			return ((ICollection<KeyValuePair<object, Pipe>>)KeyToPipe).Remove(item);
		}

		public bool TryGetValue(object key, [MaybeNullWhen(false)] out Pipe value)
		{
			return ((IDictionary<object, Pipe>)KeyToPipe).TryGetValue(key, out value);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)KeyToPipe).GetEnumerator();
		}
	}
}
