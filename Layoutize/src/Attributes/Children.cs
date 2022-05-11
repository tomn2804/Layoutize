using Layoutize.Elements;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;

namespace Layoutize.Attributes;

internal static class Children
{
    internal static IEnumerable<Layout>? Of(IBuildContext context)
    {
        Element element = context.Element;
        Debug.Assert(!element.IsDisposed);
        return Of(element.Layout);
    }

    internal static IEnumerable<Layout>? Of(Layout layout)
    {
        return layout.GetValue<object?>(nameof(Children)) switch
        {
            Layout child => new[] { child },
            IEnumerable<object> children => children.Cast<Layout>(),
            _ => null,
        };
    }

    internal static ImmutableSortedSet<Element> Of(ViewGroupElement element)
    {
        Debug.Assert(!element.IsDisposed);
        IEnumerable<Layout> children = Of(element.Layout) ?? ImmutableSortedSet<Layout>.Empty;
        return children.Select(layout => layout.CreateElement()).ToImmutableSortedSet();
    }

    internal static string RequireOf(IBuildContext context)
    {
        Element element = context.Element;
        Debug.Assert(!element.IsDisposed);
        return RequireOf(element.Layout);
    }

    internal static string RequireOf(Layout layout)
    {
        return layout.RequireValue<object>(nameof(Children)).ToString()!;
    }
}
