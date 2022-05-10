using Layoutize.Elements;
using System.Management.Automation;

namespace Layoutize.Attributes;

internal static class OnMounted
{
    internal static ScriptBlock? Of(IBuildContext context)
    {
        return Of(context.Element.Layout);
    }

    internal static ScriptBlock? Of(Layout layout)
    {
        return layout.GetValue<ScriptBlock?>(nameof(OnMounted));
    }
}
