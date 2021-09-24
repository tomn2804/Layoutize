using System;

namespace SchemataPreview
{
	public static class ObjectExtension
	{
		public static T[] ToArray<T>(this object obj)
		{
			return obj is object[] array ? Array.ConvertAll(array, (x) => (T)x) : new T[] { (T)obj };
		}
	}
}
