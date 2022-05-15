using System.Collections.Immutable;

namespace Layoutize.Attributes;

public static class DeleteOnUnmount
{
    public static bool? Of(IImmutableDictionary<object, object?> attributes)
    {
        object? value = attributes.GetValue(nameof(DeleteOnUnmount));
        return value != null ? Cast(value) : null;
    }

    public static bool RequireOf(IImmutableDictionary<object, object?> attributes)
    {
        return Cast(attributes.RequireValue(nameof(DeleteOnUnmount)));
    }

    private static bool Cast(object value)
    {
        return (bool)value;
    }
}
