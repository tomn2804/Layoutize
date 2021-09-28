using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace SchemataPreview
{
	public static class ObjectExtension
	{
		public static T AssertNotNull<T>(this T? @this)
		{
			Debug.Assert(@this is not null);
			return @this;
		}

		public static T[] ToArray<T>(this object? @this)
		{
			if (@this is null)
			{
				return Array.Empty<T>();
			}
			return @this is object[] array ? Array.ConvertAll(array, (t) => (T)t) : new T[] { (T)@this };
		}

		public static bool TryCast<T>(this object? @this, out T? result)
		{
			if (@this is T t)
			{
				result = t;
				return true;
			}
			result = default;
			return false;
		}
	}
}
