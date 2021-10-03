using System;
using System.Diagnostics;

namespace SchemataPreview
{
    internal static class GenericExtension
    {
        internal static T AssertNotNull<T>(this T? @this)
        {
            Debug.Assert(@this is not null);
            return @this;
        }
    }
}
