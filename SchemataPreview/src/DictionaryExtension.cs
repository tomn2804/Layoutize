using System.Collections.Generic;
using System.Collections.Immutable;

namespace SchemataPreview
{
	public static class DictionaryExtension
	{
		public static TValue? TryGetValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
		{
			dictionary.TryGetValue(key, out TValue? result);
			return result;
		}

		public static TValue? TryGetValue<TKey, TValue>(this IImmutableDictionary<TKey, TValue> dictionary, TKey key)
		{
			dictionary.TryGetValue(key, out TValue? result);
			return result;
		}
	}
}
