using Layoutize.Elements;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;

namespace Layoutize.Attributes;

public static class Attribute
{
    public static object? GetValue(this IBuildContext context, string key)
    {
        Element element = context.Element;
        Debug.Assert(!element.IsDisposed);
        return element.Layout.Attributes.GetValue(key);
    }

    public static object? GetValue(this IImmutableDictionary<object, object?> attributes, string key)
    {
        attributes.TryGetValue(key, out object? value);
        return value;
    }

    public static object RequireValue(this IBuildContext context, string key)
    {
        Element element = context.Element;
        Debug.Assert(!element.IsDisposed);
        return element.Layout.Attributes.RequireValue(key);
    }

    public static object RequireValue(this IImmutableDictionary<object, object?> attributes, string key)
    {
        if (!attributes.TryGetValue(key, out object? value))
        {
            throw new KeyNotFoundException($"Attribute key '{key}' is not found.");
        }
        if (value == null)
        {
            throw new ArgumentNullException(nameof(attributes), $"Attribute value '{key}' is null.");
        }
        return value;
    }
}
