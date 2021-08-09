using System;
using System.Collections.Generic;

namespace SchemataPreview
{
	public class UniqueList<T> : List<T> where T : IEquatable<T>
	{
		public new T this[int index]
		{
			get => base[index];
			set => base[index] = IndexOf(value) == index ? value : throw new InvalidOperationException();
		}

		public new void Add(T item)
		{
			if (Contains(item))
			{
				throw new InvalidOperationException();
			}
			base.Add(item);
		}

		public new void Insert(int index, T item)
		{
			if (Contains(item))
			{
				throw new InvalidOperationException();
			}
			base.Insert(index, item);
		}
	}
}
