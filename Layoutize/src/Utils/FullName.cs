using Layoutize.Elements;
using System;
using System.Diagnostics;

namespace Layoutize.Utils;

public static class FullName
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
        string fullName = context.Element.View.FullName;
        Debug.Assert(IsValid(fullName));
        return fullName;
    }

    public static void Validate(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException($"Attribute value '{nameof(FullName)}' is either null, empty, or consists of only white-space characters.", nameof(value));
        }
        if (value.IndexOfAny(System.IO.Path.GetInvalidPathChars()) != -1)
        {
            throw new ArgumentException($"Attribute value '{nameof(FullName)}' contains invalid characters.", nameof(value));
        }
        if (!System.IO.Path.IsPathFullyQualified(value))
        {
            throw new ArgumentException($"Attribute value '{nameof(FullName)}' is not an absolute path.", nameof(value));
        }
    }
}
