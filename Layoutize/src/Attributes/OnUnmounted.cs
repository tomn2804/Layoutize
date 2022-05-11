using Layoutize.Elements;
using System;
using System.Management.Automation;

namespace Layoutize.Attributes;

internal static class OnUnmounted
{
    internal static EventHandler? Of(IBuildContext context)
    {
        object? value = context.GetValue(nameof(OnUnmounted));
        return value != null ? Cast(value) : null;
    }

    internal static EventHandler? Of(Layout layout)
    {
        object? value = layout.GetValue(nameof(OnUnmounted));
        return value != null ? Cast(value) : null;
    }

    internal static EventHandler RequireOf(IBuildContext context)
    {
        return Cast(context.RequireValue(nameof(OnUnmounted)));
    }

    internal static EventHandler RequireOf(Layout layout)
    {
        return Cast(layout.RequireValue(nameof(OnUnmounted)));
    }

    private static EventHandler Cast(object value)
    {
        ScriptBlock scriptBlock = (ScriptBlock)value;
        return (sender, e) => scriptBlock.Invoke(sender, e);
    }
}
