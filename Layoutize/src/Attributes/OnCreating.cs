using Layoutize.Elements;
using System;
using System.Management.Automation;

namespace Layoutize.Attributes;

internal static class OnCreating
{
    internal static EventHandler? Of(IBuildContext context)
    {
        object? value = context.GetValue(nameof(OnCreating));
        return value != null ? Cast(value) : null;
    }

    internal static EventHandler? Of(Layout layout)
    {
        object? value = layout.GetValue(nameof(OnCreating));
        return value != null ? Cast(value) : null;
    }

    internal static EventHandler RequireOf(IBuildContext context)
    {
        return Cast(context.RequireValue(nameof(OnCreating)));
    }

    internal static EventHandler RequireOf(Layout layout)
    {
        return Cast(layout.RequireValue(nameof(OnCreating)));
    }

    private static EventHandler Cast(object value)
    {
        ScriptBlock scriptBlock = (ScriptBlock)value;
        return (sender, e) => scriptBlock.Invoke(sender, e);
    }
}
