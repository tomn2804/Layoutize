using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Layoutize.Layouts;

internal static class Model
{
	public static bool IsValid([NotNullWhen(true)] object? value)
	{
		return value != null && Validator.TryValidateObject(value, new(value), null);
	}

	public static void Validate([NotNull] object? value)
	{
		Debug.Assert(value != null);
		Validator.ValidateObject(value, new(value), true);
	}
}
