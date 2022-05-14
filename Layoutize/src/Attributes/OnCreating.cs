using Layoutize.Elements;
using System;
using System.Collections.Immutable;
using System.Management.Automation;

namespace Layoutize.Attributes;

public static class OnCreating
{
    public static EventHandler? Of(IBuildContext context)
    {
        object? value = context.GetValue(nameof(OnCreating));
        return value != null ? Cast(value) : null;
    }

    public static EventHandler? Of(IImmutableDictionary<object, object?> attributes)
    {
        object? value = attributes.GetValue(nameof(OnCreating));
        return value != null ? Cast(value) : null;
    }

    public static EventHandler RequireOf(IBuildContext context)
    {
        return Cast(context.RequireValue(nameof(OnCreating)));
    }

    public static EventHandler RequireOf(IImmutableDictionary<object, object?> attributes)
    {
        return Cast(attributes.RequireValue(nameof(OnCreating)));
    }

    private static EventHandler Cast(object value)
    {
        ScriptBlock scriptBlock = (ScriptBlock)value;
        return (sender, e) => scriptBlock.Invoke(sender, e);
    }
}
