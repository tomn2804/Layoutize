using System.Collections;

namespace SchemataPreview
{
    public static class HashtableExtension
    {
        public static ImmutableHashtable ToImmutable(this Hashtable @this)
        {
            return new(@this);
        }
    }
}
