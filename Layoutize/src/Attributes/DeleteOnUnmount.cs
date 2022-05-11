using Layoutize.Elements;
using System.Diagnostics;

namespace Layoutize.Attributes;

internal static class DeleteOnUnmount
{
    internal static bool? Of(IBuildContext context)
    {
        Element element = context.Element;
        Debug.Assert(!element.IsDisposed);
        return Of(element.Layout);
    }

    internal static bool? Of(Layout layout)
    {
        return layout.GetValue<bool?>(nameof(DeleteOnUnmount));
    }

    internal static bool RequireOf(IBuildContext context)
    {
        Element element = context.Element;
        Debug.Assert(!element.IsDisposed);
        return RequireOf(element.Layout);
    }

    internal static bool RequireOf(Layout layout)
    {
        return layout.RequireValue<bool>(nameof(DeleteOnUnmount));
    }
}
