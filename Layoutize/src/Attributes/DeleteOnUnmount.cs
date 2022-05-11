using Layoutize.Elements;

namespace Layoutize.Attributes;

internal static class DeleteOnUnmount
{
    internal static bool? Of(IBuildContext context)
    {
        object? value = context.GetValue(nameof(DeleteOnUnmount));
        return value != null ? Cast(value) : null;
    }

    internal static bool? Of(Layout layout)
    {
        object? value = layout.GetValue(nameof(DeleteOnUnmount));
        return value != null ? Cast(value) : null;
    }

    internal static bool RequireOf(IBuildContext context)
    {
        return Cast(context.RequireValue(nameof(DeleteOnUnmount)));
    }

    internal static bool RequireOf(Layout layout)
    {
        return Cast(layout.RequireValue(nameof(DeleteOnUnmount)));
    }

    private static bool Cast(object value)
    {
        return (bool)value;
    }
}
