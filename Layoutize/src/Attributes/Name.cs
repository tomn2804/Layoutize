using Layoutize.Elements;
using System;

namespace Layoutize.Attributes;

internal static class Name
{
    internal static bool IsValid(string name)
    {
        return !string.IsNullOrWhiteSpace(name) && (name.IndexOfAny(System.IO.Path.GetInvalidFileNameChars()) == -1);
    }

    internal static string? Of(IBuildContext context)
    {
        object? value = context.GetValue(nameof(Name));
        return value != null ? Cast(value) : null;
    }

    internal static string? Of(Layout layout)
    {
        object? value = layout.GetValue(nameof(Name));
        return value != null ? Cast(value) : null;
    }

    internal static string RequireOf(IBuildContext context)
    {
        return Cast(context.RequireValue(nameof(Name)));
    }

    internal static string RequireOf(Layout layout)
    {
        return Cast(layout.RequireValue(nameof(Name)));
    }

    private static string Cast(object value)
    {
        string name = value.ToString()!;
        if (!IsValid(name))
        {
            throw new ArgumentException($"'{nameof(Name)}' attribute value is invalid.", nameof(value));
        }
        return name;
    }
}
