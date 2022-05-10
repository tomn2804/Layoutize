using Layoutize.Elements;

namespace Layoutize.Attributes;

internal static class Name
{
    internal static string Of(IBuildContext context)
    {
        return Of(context.Element.Layout);
    }

    internal static string Of(Layout layout)
    {
        return layout.RequireValue<object>(nameof(Name)).ToString()!;
    }
}
