using System;
using System.Diagnostics;

namespace Layoutize.Elements;

public static class Name
{
    public static bool IsValid(string name)
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

    public static string RequireOf(IBuildContext context)
    {
        Element element = context.Element;
        string name = element.View.Name;
        Debug.Assert(IsValid(name));
        return name;
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
