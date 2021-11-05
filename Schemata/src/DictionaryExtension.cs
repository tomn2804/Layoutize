using System.Collections;
using System.Collections.Generic;

namespace Schemata
{
    public static class DictionaryExtension
    {
        public static IDictionary MergeTo(this IDictionary @this, IEnumerable<DictionaryEntry> entries)
        {
            foreach (DictionaryEntry entry in entries)
            {
                if (!@this.Contains(entry.Key))
                {
                    @this.Add(entry.Key, entry.Value);
                }
            }
            return @this;
        }

        public static IDictionary<TKey, TValue> MergeTo<TKey, TValue>(this IDictionary @this, IEnumerable<KeyValuePair<TKey, TValue>> other)
        {
        }

        public static IDictionary MergeTo(this IDictionary @this, IEnumerable<DictionaryEntry> other)
        {
        }

        public static IDictionary<TKey, TValue> MergeTo<TKey, TValue>(this IDictionary @this, IEnumerable<KeyValuePair<TKey, TValue>> other)
        {
        }
    }
}
