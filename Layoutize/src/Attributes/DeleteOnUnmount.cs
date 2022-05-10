using Layoutize.Elements;

namespace Layoutize.Attributes;

internal static class DeleteOnUnmount
{
    internal static bool? Of(IBuildContext context)
    {
        return Of(context.Element.Layout);
    }

    internal static bool? Of(Layout layout)
    {
        return layout.GetValue<bool?>(nameof(DeleteOnUnmount));
    }
}
