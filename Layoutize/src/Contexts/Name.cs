using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Layoutize.Elements;

namespace Layoutize.Contexts;

public static class Name
{
	public static string Of(IBuildContext context)
	{
		var element = context.Element;
		if (!element.IsMounted) throw new ArgumentException("Context is not mounted.", nameof(context));
		var name = element.View.Name;
		Debug.Assert(TryValidate(name));
		return name;
	}

	public static bool TryValidate([NotNullWhen(true)] string? value)
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

	public static void Validate([NotNull] string? value)
	{
		if (string.IsNullOrWhiteSpace(value))
		{
			throw new ValidationException(
				$"Attribute value '{nameof(Name)}' is either null, empty, or consists of only white-space characters."
			);
		}
		if (value.IndexOfAny(System.IO.Path.GetInvalidFileNameChars()) != -1)
		{
			throw new ValidationException($"Attribute value '{nameof(Name)}' contains invalid characters.");
		}
	}
}
