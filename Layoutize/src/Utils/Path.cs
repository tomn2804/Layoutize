using Layoutize.Elements;
using System.IO;

namespace Layoutize.Utils;

internal static class Path
{
    internal static string Of(IBuildContext context)
    {
        string path = Directory.GetCurrentDirectory();
        Element.Visitor visitor = null!;
        visitor = parent =>
        {
            if (parent is ViewElement element)
            {
                path = element.View.FullName;
            }
            else
            {
                parent.VisitParent(visitor);
            }
        };
        context.Element.VisitParent(visitor);
        return path;
    }
}
