using Layoutize.Elements;
using System.Diagnostics;

namespace Layoutize.Utils;

internal static class Name
{
    internal static string Of(IBuildContext context)
    {
        Element element = context.Element;
        Debug.Assert(!element.IsDisposed);
        return element.Name;
    }
}
