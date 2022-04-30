using Layoutize.Elements;
using System.IO;

namespace Layoutize.Utils;

internal static class Path
{
    internal static string Of(IBuildContext context)
    {
        string path = Directory.GetCurrentDirectory();
        void visitParent(Element? element)
        {
            if (element is ViewElement parent)
            {
                path = parent.View.FullName;
            }
            else
            {
                visitParent(element?.Parent);
            }
        }
        visitParent(context.Element);
        return path;
    }
}
