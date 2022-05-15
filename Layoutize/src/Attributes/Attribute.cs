using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Layoutize.Attributes;

public static class Attribute
{
    public static object? GetValue(this IImmutableDictionary<object, object?> attributes, string key)
    {
        attributes.TryGetValue(key, out object? value);
        return value;
    }

    public static object RequireValue(this IImmutableDictionary<object, object?> attributes, string key)
    {
        if (!attributes.TryGetValue(key, out object? value))
        {
            throw new KeyNotFoundException($"Attribute key '{key}' is missing.");
        }
        if (value == null)
        {
            throw new ArgumentNullException(nameof(attributes), $"Attribute value '{key}' is null.");
        }
        return value;
    }
}
