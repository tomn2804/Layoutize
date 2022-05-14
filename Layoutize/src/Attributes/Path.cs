using Layoutize.Elements;
using System;
using System.Collections.Immutable;
using System.Diagnostics;

namespace Layoutize.Attributes;

public static class Path
{
    public static string? Of(IBuildContext context)
    {
        Element element = context.Element;
        Debug.Assert(!element.IsDisposed);
        string? path = null;
        void visitParent(Element element)
        {
            Debug.Assert(!element.IsDisposed);
            Element? parent = element.Parent;
            if (parent != null)
            {
                Debug.Assert(!parent.IsDisposed);
                Debug.Assert(parent.IsMounted);
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
        }
        visitParent(element);
        if (path == null)
        {
            path = Of(element.Layout.Attributes);
        }
        Debug.Assert(path != null && TryValidate(path));
        return path;
    }

    public static string? Of(IImmutableDictionary<object, object?> attributes)
    {
        object? value = attributes.GetValue(nameof(Path));
        if (value != null)
        {
            string path = Cast(value);
            Debug.Assert(TryValidate(path));
            return path;
        }
        return null;
    }

    public static string RequireOf(IBuildContext context)
    {
        string path = Of(context) ?? Of(context.Element)!;
        Debug.Assert(TryValidate(path));
        return path;
    }

    public static string RequireOf(IImmutableDictionary<object, object?> attributes)
    {
        string path = Cast(attributes.RequireValue(nameof(Path)));
        Debug.Assert(TryValidate(path));
        return path;
    }

    public static bool TryValidate(string path)
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

    public static void Validate(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            throw new ArgumentException($"Attribute value '{nameof(Path)}' cannot be null or contains only white spaces.", nameof(path));
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
