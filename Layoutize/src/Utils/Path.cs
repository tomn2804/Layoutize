using Layoutize.Elements;
using System.IO;

namespace Layoutize.Utils;

internal static class Path
{
    internal static string Of(IBuildContext context)
    {
        string path = Directory.GetCurrentDirectory();
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
        visitParent(context.Element);
        return path;
    }
}
