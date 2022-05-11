using Layoutize.Elements;
using System;
using System.Diagnostics;

namespace Layoutize.Attributes;

internal static class Path
{
    internal static bool ContainsInvalidChars(string path)
    {
        return path.IndexOfAny(System.IO.Path.GetInvalidPathChars()) != -1;
    }

    internal static bool IsValid(string path)
    {
        return !ContainsInvalidChars(path) && System.IO.Path.IsPathFullyQualified(path);
    }

    internal static string? Of(IBuildContext context)
    {
        Element element = context.Element;
        if (element.IsDisposed)
        {
            throw new ObjectDisposedException(nameof(IBuildContext));
        }
        string? path = null;
        void visitParent(Element element)
        {
            Element? parent = element.Parent;
            switch (parent)
            {
                case ViewElement:
                    path = parent.View.FullName;
                    return;

                case Element:
                    visitParent(parent);
                    return;

                default:
                    return;
            }
        }
        visitParent(element);
        if (path == null)
        {
            path = Of(element.Layout);
        }
        else
        {
            Debug.Assert(IsValid(path));
        }
        return path;
    }

    internal static string? Of(Layout layout)
    {
        object? value = layout.GetValue(nameof(Path));
        return value != null ? Cast(value) : null;
    }

    internal static string RequireOf(IBuildContext context)
    {
        string? path = Of(context);
        if (path == null)
        {
            return RequireOf(context.Element.Layout);
        }
        Debug.Assert(IsValid(path));
        return path;
    }

    internal static string RequireOf(Layout layout)
    {
        return Cast(layout.RequireValue(nameof(Path)));
    }

    private static string Cast(object value)
    {
        string path = value.ToString()!;
        if (!IsValid(path))
        {
            throw new ArgumentException($"Value of '{nameof(Path)}' attribute is invalid.", nameof(value));
        }
        Debug.Assert(System.IO.Path.IsPathFullyQualified(path));
        return System.IO.Path.GetFullPath(path);
    }
}
