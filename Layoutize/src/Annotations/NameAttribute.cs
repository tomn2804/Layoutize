using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace Layoutize.Annotations;

internal sealed class NameAttribute : LayoutAttribute
{
	internal static bool IsValid([NotNullWhen(true)] string? value)
	{
		try
		{
			Validate(value);
			return true;
		}
		catch
		{
			return false;
		}
	}

	protected override void Validate(object? value)
	{
		Validate(value as string);
	}

	internal static void Validate([NotNull] string? value)
	{
		if (string.IsNullOrWhiteSpace(value))
		{
			throw new ValidationException(
				$"'{nameof(NameAttribute)}' value is either null, empty, or consists of only white-space characters."
			);
		}
		if (value.IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
		{
			throw new ValidationException($"'{nameof(NameAttribute)}' value contains invalid characters.");
		}
	}
}
