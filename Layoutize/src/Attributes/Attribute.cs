using Layoutize.Elements;
using System;
using System.Collections.Generic;

namespace Layoutize.Attributes;

internal static class Attribute
{
    internal static object? GetValue(this IBuildContext context, string key)
    {
        Element element = context.Element;
        if (element.IsDisposed)
        {
            throw new ObjectDisposedException(nameof(IBuildContext));
        }
        return element.Layout.GetValue(key);
    }

    internal static object? GetValue(this Layout layout, string key)
    {
        layout.Attributes.TryGetValue(key, out object? value);
        return value;
    }

    internal static object RequireValue(this IBuildContext context, string key)
    {
        Element element = context.Element;
        if (element.IsDisposed)
        {
            throw new ObjectDisposedException(nameof(IBuildContext));
        }
        return element.Layout.RequireValue(key);
    }

    internal static object RequireValue(this Layout layout, string key)
    {
        if (!layout.Attributes.TryGetValue(key, out object? value))
        {
            throw new KeyNotFoundException($"'{key}' attribute was not found.");
        }
        if (value == null)
        {
            throw new ArgumentNullException(nameof(layout), $"'{key}' attribute value is null.");
        }
        return value;
    }
}
