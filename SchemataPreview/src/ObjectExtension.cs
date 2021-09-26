using System;

namespace SchemataPreview
{
	public static class ObjectExtension
	{
		public static T[] ToArray<T>(this object? @object)
		{
			if (@object is not null)
			{
				return @object is object[] array ? Array.ConvertAll(array, (t) => (T)t) : new T[] { (T)@object };
			}
			return Array.Empty<T>();
		}
	}
}
