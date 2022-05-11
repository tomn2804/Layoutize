using Layoutize.Elements;
using System.Diagnostics;

namespace Layoutize.Attributes;

internal static class Name
{
    internal static string? Of(IBuildContext context)
    {
        Element element = context.Element;
        Debug.Assert(!element.IsDisposed);
        return Of(element.Layout);
    }

    internal static string? Of(Layout layout)
    {
        return layout.GetValue<object>(nameof(Name))?.ToString();
    }

    internal static string RequireOf(IBuildContext context)
    {
        Element element = context.Element;
        Debug.Assert(!element.IsDisposed);
        return RequireOf(element.Layout);
    }

    internal static string RequireOf(Layout layout)
    {
        return layout.RequireValue<object>(nameof(Name)).ToString()!;
    }
}
