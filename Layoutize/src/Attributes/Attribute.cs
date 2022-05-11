using System.Collections.Generic;
using System.Diagnostics;

namespace Layoutize.Attributes;

internal static class Attribute
{
    internal static T? GetValue<T>(this Layout layout, string key)
    {
        if (layout.Attributes.TryGetValue(key, out object? value))
        {
            Debug.Assert(value != null);
            return (T)value;
        }
        return default;
    }

    internal static T RequireValue<T>(this Layout layout, string key)
    {
        T? value = layout.GetValue<T>(key);
        if (value == null)
        {
            throw new KeyNotFoundException($"Layout does not contain attribute '{key}'.");
        }
        return value;
    }
}
