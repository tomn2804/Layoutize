using Layoutize.Elements;
using System;
using System.Diagnostics;

namespace Layoutize.Utils;

public static class Name
{
    public static bool IsValid(string value)
    {
        try
        {
            Validate(value);
        }
        catch
        {
            return false;
        }
        return true;
    }

    public static string Of(IBuildContext context)
    {
        string name = context.Element.View.Name;
        Debug.Assert(IsValid(name));
        return name;
    }

    public static void Validate(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException($"Attribute value '{nameof(Name)}' is either null, empty, or consists of only white-space characters.", nameof(value));
        }
        if (value.IndexOfAny(System.IO.Path.GetInvalidFileNameChars()) != -1)
        {
            throw new ArgumentException($"Attribute value '{nameof(Name)}' contains invalid characters.", nameof(value));
        }
    }
}
