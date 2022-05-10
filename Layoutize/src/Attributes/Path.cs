using Layoutize.Elements;
using System.Diagnostics;

namespace Layoutize.Attributes;

internal static class Path
{
    internal static string Of(IBuildContext context)
    {
        Element element = context.Element;
        Debug.Assert(!element.IsDisposed);
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
        Debug.Assert(path != null);
        Debug.Assert(System.IO.Path.IsPathFullyQualified(path));
        return System.IO.Path.GetFullPath(path);
    }

    internal static string Of(Layout layout)
    {
        return layout.RequireValue<string>(nameof(Path));
    }
}
