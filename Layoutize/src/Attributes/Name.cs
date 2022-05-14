using Layoutize.Elements;
using System;
using System.Collections.Immutable;
using System.Diagnostics;

namespace Layoutize.Attributes;

public static class Name
{
    public static string? Of(IBuildContext context)
    {
        object? value = context.GetValue(nameof(Name));
        if (value != null)
        {
            string name = Cast(value);
            Debug.Assert(TryValidate(name));
            return name;
        }
        return null;
    }

    public static string? Of(IImmutableDictionary<object, object?> attributes)
    {
        object? value = attributes.GetValue(nameof(Name));
        if (value != null)
        {
            string name = Cast(value);
            Debug.Assert(TryValidate(name));
            return name;
        }
        return null;
    }

    public static string RequireOf(IBuildContext context)
    {
        string name = Cast(context.RequireValue(nameof(Path)));
        Debug.Assert(TryValidate(name));
        return name;
    }

    public static string RequireOf(IImmutableDictionary<object, object?> attributes)
    {
        string name = Cast(attributes.RequireValue(nameof(Path)));
        Debug.Assert(TryValidate(name));
        return name;
    }

    public static bool TryValidate(string name)
    {
        try
        {
            Validate(name);
        }
        catch
        {
            return false;
        }
        return true;
    }

    public static void Validate(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException($"Attribute value '{nameof(Name)}' is null or contains only white spaces.", nameof(name));
        }
        if (name.IndexOfAny(System.IO.Path.GetInvalidFileNameChars()) != -1)
        {
            throw new ArgumentException($"Attribute value '{nameof(Name)}' contains invalid characters.", nameof(name));
        }
    }

    private static string Cast(object value)
    {
        string name = value.ToString()!;
        Validate(name);
        return name;
    }
}
