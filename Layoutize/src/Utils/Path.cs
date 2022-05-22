using Layoutize.Elements;
using System;
using System.Diagnostics;

namespace Layoutize.Utils;

public static class Path
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
        string path = string.Empty;
        void visitParent(Element element)
        {
            Element? parent = element.Parent;
            switch (parent)
            {
                case ViewElement:
                    path = FullName.Of(parent);
                    return;

                case Element:
                    visitParent(parent);
                    return;

                default:
                    return;
            }
        }
        visitParent(context.Element);
        Debug.Assert(IsValid(path));
        return path;
    }

    public static void Validate(string value)
    {
        if (value.IndexOfAny(System.IO.Path.GetInvalidPathChars()) != -1)
        {
            throw new ArgumentException($"Attribute value '{nameof(Path)}' contains invalid characters.", nameof(value));
        }
    }
}
