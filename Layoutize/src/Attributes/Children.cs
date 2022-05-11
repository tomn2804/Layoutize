using Layoutize.Elements;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Layoutize.Attributes;

internal static class Children
{
    internal static ImmutableSortedSet<Element>? Of(IBuildContext context)
    {
        object? value = context.GetValue(nameof(Children));
        return value != null ? Cast(value) : null;
    }

    internal static ImmutableSortedSet<Element>? Of(Layout layout)
    {
        object? value = layout.GetValue(nameof(Children));
        return value != null ? Cast(value) : null;
    }

    internal static ImmutableSortedSet<Element> RequireOf(IBuildContext context)
    {
        return Cast(context.RequireValue(nameof(Children)));
    }

    internal static ImmutableSortedSet<Element> RequireOf(Layout layout)
    {
        return Cast(layout.RequireValue(nameof(Children)));
    }

    private static ImmutableSortedSet<Element> Cast(object value)
    {
        return value switch
        {
            IEnumerable<object> children => children.Cast<Layout>().Select(child => child.CreateElement()).ToImmutableSortedSet(),
            _ => ImmutableSortedSet.Create(((Layout)value).CreateElement()),
        };
    }
}
