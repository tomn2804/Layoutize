using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Layoutize.Elements;

namespace Layoutize.Contexts;

public static class Name
{
	public static string Of(IBuildContext context)
	{
		return context.Element.View.Name;
	}

	internal static bool IsValid([NotNullWhen(true)] string? value)
	{
		try
		{
			Validate(value);
		}
		catch
		{
			return false;
		}
		return true;
	}

	internal static void Validate([NotNull] string? value)
	{
		if (string.IsNullOrWhiteSpace(value))
		{
			throw new ValidationException(
				$"Layout property value '{nameof(Name)}' is either null, empty, or consists of only white-space characters."
			);
		}
		if (value.IndexOfAny(System.IO.Path.GetInvalidFileNameChars()) != -1)
		{
			throw new ValidationException($"Layout property value '{nameof(Name)}' contains invalid characters.");
		}
	}
}
