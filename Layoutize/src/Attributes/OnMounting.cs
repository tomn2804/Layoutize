using Layoutize.Elements;
using System;
using System.Management.Automation;

namespace Layoutize.Attributes;

internal static class OnMounting
{
    internal static EventHandler? Of(IBuildContext context)
    {
        object? value = context.GetValue(nameof(OnMounting));
        return value != null ? Cast(value) : null;
    }

    internal static EventHandler? Of(Layout layout)
    {
        object? value = layout.GetValue(nameof(OnMounting));
        return value != null ? Cast(value) : null;
    }

    internal static EventHandler RequireOf(IBuildContext context)
    {
        return Cast(context.RequireValue(nameof(OnMounting)));
    }

    internal static EventHandler RequireOf(Layout layout)
    {
        return Cast(layout.RequireValue(nameof(OnMounting)));
    }

    private static EventHandler Cast(object value)
    {
        ScriptBlock scriptBlock = (ScriptBlock)value;
        return (sender, e) => scriptBlock.Invoke(sender, e);
    }
}
