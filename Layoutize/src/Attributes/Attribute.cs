using Layoutize.Elements;
using System.Diagnostics;

namespace Layoutize.Attributes;

internal static class Attribute
{
    internal static T? GetValue<T>(this IBuildContext context, string key)
    {
        Element element = context.Element;
        Debug.Assert(!element.IsDisposed);
        return element.Layout.GetValue<T>(key);
    }

    internal static T? GetValue<T>(this Layout layout, string key)
    {
        if (layout.Attributes.TryGetValue(key, out object? value))
        {
            Debug.Assert(value != null);
            return (T)value;
        }
        return default;
    }

    internal static T RequireValue<T>(this IBuildContext context, string key)
    {
        Element element = context.Element;
        Debug.Assert(!element.IsDisposed);
        return element.Layout.RequireValue<T>(key);
    }

    internal static T RequireValue<T>(this Layout layout, string key)
    {
        object? value = layout.Attributes[key];
        Debug.Assert(value != null);
        return (T)value;
    }
}
