using Layoutize.Elements;
using System;
using System.Diagnostics;

namespace Layoutize.Views;

public static class Path
{
    public static bool IsValid(string path)
    {
        try
        {
            Validate(path);
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
        string? path = null;
        void visitParent(Element element)
        {
            Element? parent = element.Parent;
            Debug.Assert(parent != null);
            switch (parent)
            {
                case ViewElement:
                    path = parent.View.FullName;
                    return;

                default:
                    visitParent(parent);
                    return;
            }
        }
        visitParent(element);
        Debug.Assert(path != null);
        Debug.Assert(IsValid(path));
        return path;
    }

    public static void Validate(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            throw new ArgumentException($"Attribute value '{nameof(Path)}' is null or contains only white spaces.", nameof(path));
        }
        if (path.IndexOfAny(System.IO.Path.GetInvalidPathChars()) != -1)
        {
            throw new ArgumentException($"Attribute value '{nameof(Path)}' contains invalid characters.", nameof(path));
        }
        if (!System.IO.Path.IsPathFullyQualified(path))
        {
            throw new ArgumentException($"Attribute value '{nameof(Path)}' is not an absolute path.", nameof(path));
        }
    }

    private static string Cast(object value)
    {
        string path = value.ToString()!;
        Validate(path);
        return path;
    }
}
