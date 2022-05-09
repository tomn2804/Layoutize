using Layoutize.Elements;
using System.Diagnostics;

namespace Layoutize.Utils;

internal static class Path
{
    internal static string Of(IBuildContext context)
    {
        Debug.Assert(context.Element.Parent != null);
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
        visitParent(context.Element);
        Debug.Assert(path != null);
        return path;
    }
}
