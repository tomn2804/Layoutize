using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Layoutize.Elements;

namespace Layoutize.Contexts;

public static class FullName
{
	public static string Of(IBuildContext context)
	{
		var element = context.Element;
		if (!element.IsMounted) throw new ElementNotMountedException(element);
		var fullName = element.View.FullName;
		Debug.Assert(IsValid(fullName));
		return fullName;
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
				$"Layout property value '{nameof(FullName)}' is either null, empty, or consists of only white-space characters."
			);
		}
		if (value.IndexOfAny(System.IO.Path.GetInvalidPathChars()) != -1)
		{
			throw new ValidationException($"Layout property value '{nameof(FullName)}' contains invalid characters.");
		}
		if (!System.IO.Path.IsPathFullyQualified(value))
		{
			throw new ValidationException($"Layout property value '{nameof(FullName)}' is not an absolute path.");
		}
	}
}
