using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Layoutize.Elements;
using Layoutize.Views;

namespace Layoutize.Contexts;

public static class Name
{
	public static string Of(IBuildContext context)
	{
		var element = context.Element;
		if (!element.IsMounted) throw new ElementNotMountedException(element);
		return Of(element.ViewContext);
	}

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

	internal static string Of(IViewContext context)
	{
		var name = context.Name;
		Debug.Assert(IsValid(name));
		return name;
	}

	internal static void Validate([NotNull] string? value)
	{
		if (string.IsNullOrWhiteSpace(value))
		{
			throw new ValidationException(
				$"Property value '{nameof(Name)}' is either null, empty, or consists of only white-space characters."
			);
		}
		if (value.IndexOfAny(System.IO.Path.GetInvalidFileNameChars()) != -1)
		{
			throw new ValidationException($"Property value '{nameof(Name)}' contains invalid characters.");
		}
	}
}
