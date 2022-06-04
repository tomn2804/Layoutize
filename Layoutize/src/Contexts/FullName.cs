﻿using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Layoutize.Elements;
using Layoutize.Views;

namespace Layoutize.Contexts;

public static class FullName
{
	public static string Of(IBuildContext context)
	{
		var element = context.Element;
		if (!element.IsMounted) throw new ElementNotMountedException(element);
		var view = element.View;
		Debug.Assert(view != null);
		return Of(view);
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

	internal static void Validate([NotNull] string? value)
	{
		if (string.IsNullOrWhiteSpace(value))
		{
			throw new ValidationException(
				$"'{nameof(FullName)}' value is either null, empty, or consists of only white-space characters."
			);
		}
		if (value.IndexOfAny(System.IO.Path.GetInvalidPathChars()) != -1)
		{
			throw new ValidationException($"'{nameof(FullName)}' value contains invalid characters.");
		}
		if (!System.IO.Path.IsPathFullyQualified(value))
		{
			throw new ValidationException($"'{nameof(FullName)}' value is not an absolute path.");
		}
	}

	private static string Of(IView view)
	{
		var fullName = view.FullName;
		Debug.Assert(IsValid(fullName));
		return fullName;
	}
}
