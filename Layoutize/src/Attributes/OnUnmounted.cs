using Layoutize.Elements;
using System.Diagnostics;
using System.Management.Automation;

namespace Layoutize.Attributes;

internal static class OnUnmounted
{
    internal static ScriptBlock? Of(IBuildContext context)
    {
        Element element = context.Element;
        Debug.Assert(!element.IsDisposed);
        return Of(element.Layout);
    }

    internal static ScriptBlock? Of(Layout layout)
    {
        return layout.GetValue<ScriptBlock?>(nameof(OnUnmounted));
    }

    internal static ScriptBlock RequireOf(IBuildContext context)
    {
        Element element = context.Element;
        Debug.Assert(!element.IsDisposed);
        return RequireOf(element.Layout);
    }

    internal static ScriptBlock RequireOf(Layout layout)
    {
        return layout.RequireValue<ScriptBlock>(nameof(OnUnmounted));
    }
}
