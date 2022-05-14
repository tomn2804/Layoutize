using Layoutize.Elements;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Layoutize.Attributes;

public static class Children
{
    public static IEnumerable<Layout>? Of(IBuildContext context)
    {
        object? value = context.GetValue(nameof(Children));
        return value != null ? Cast(value) : null;
    }

    public static IEnumerable<Layout>? Of(IImmutableDictionary<object, object?> attributes)
    {
        object? value = attributes.GetValue(nameof(Children));
        return value != null ? Cast(value) : null;
    }

    public static IEnumerable<Layout> RequireOf(IBuildContext context)
    {
        return Cast(context.RequireValue(nameof(Children)));
    }

    public static IEnumerable<Layout> RequireOf(IImmutableDictionary<object, object?> attributes)
    {
        return Cast(attributes.RequireValue(nameof(Children)));
    }

    private static IEnumerable<Layout> Cast(object value)
    {
        return value switch
        {
            IEnumerable<object> children => children.Cast<Layout>(),
            _ => new[] { (Layout)value },
        };
    }
}
