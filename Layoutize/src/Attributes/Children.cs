using Layoutize.Elements;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Layoutize.Attributes;

internal static class Children
{
    internal static IEnumerable<Layout>? Of(IBuildContext context)
    {
        return Of(context.Element.Layout);
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
        return element.GetValue<object?>(nameof(Children)) switch
        {
            Layout child => ImmutableSortedSet.Create(child.CreateElement()),
            IEnumerable<object> children => children.Cast<Layout>().Select(layout => layout.CreateElement()).ToImmutableSortedSet(),
            _ => ImmutableSortedSet<Element>.Empty,
        };
    }
}
