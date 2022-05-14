using Layoutize.Elements;
using System;
using System.Collections.Immutable;
using System.Management.Automation;

namespace Layoutize.Attributes;

public static class OnCreated
{
    public static EventHandler? Of(IBuildContext context)
    {
        object? value = context.GetValue(nameof(OnCreated));
        return value != null ? Cast(value) : null;
    }

    public static EventHandler? Of(IImmutableDictionary<object, object?> attributes)
    {
        object? value = attributes.GetValue(nameof(OnCreated));
        return value != null ? Cast(value) : null;
    }

    public static EventHandler RequireOf(IBuildContext context)
    {
        return Cast(context.RequireValue(nameof(OnCreated)));
    }

    public static EventHandler RequireOf(IImmutableDictionary<object, object?> attributes)
    {
        return Cast(attributes.RequireValue(nameof(OnCreated)));
    }

    private static EventHandler Cast(object value)
    {
        ScriptBlock scriptBlock = (ScriptBlock)value;
        return (sender, e) => scriptBlock.Invoke(sender, e);
    }
}
