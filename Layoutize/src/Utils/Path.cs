using Layoutize.Elements;
using System.Diagnostics;

namespace Layoutize.Utils;

internal static class Path
{
    internal static string Of(IBuildContext context)
    {
        Element element = context.Element;
        Debug.Assert(!element.IsDisposed);
        string path = null!;
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
        Debug.Assert(System.IO.Path.IsPathFullyQualified(path));
        return System.IO.Path.GetFullPath(path);
    }
}
