using System;
using System.Collections.Immutable;
using System.Management.Automation;

namespace Layoutize.Attributes;

public static class OnDeleted
{
    public static EventHandler? Of(IImmutableDictionary<object, object?> attributes)
    {
        object? value = attributes.GetValue(nameof(OnDeleted));
        return value != null ? Cast(value) : null;
    }

    public static EventHandler RequireOf(IImmutableDictionary<object, object?> attributes)
    {
        return Cast(attributes.RequireValue(nameof(OnDeleted)));
    }

    private static EventHandler Cast(object value)
    {
        ScriptBlock scriptBlock = (ScriptBlock)value;
        return (sender, e) => scriptBlock.Invoke(sender, e);
    }
}
