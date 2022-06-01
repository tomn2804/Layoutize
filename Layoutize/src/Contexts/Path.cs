using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Layoutize.Elements;

namespace Layoutize.Contexts;

public static class Path
{
	public static string Of(IBuildContext context)
	{
		var path = string.Empty;
		void VisitParent(Element element)
		{
			var parent = element.Parent;
			switch (parent)
			{
				case ViewElement viewElement:
					path = viewElement.View.FullName;
					return;

				case not null:
					VisitParent(parent);
					return;

				default:
					return;
			}
		}
		VisitParent(context.Element);
		Debug.Assert(IsValid(path));
		return path;
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
		if (value == null)
		{
			throw new ValidationException($"Layout property value '{nameof(Path)}' is null.");
		}
		if (value.IndexOfAny(System.IO.Path.GetInvalidPathChars()) != -1)
		{
			throw new ValidationException($"Layout property value '{nameof(Path)}' contains invalid characters.");
		}
		if (!string.IsNullOrWhiteSpace(value) && !System.IO.Path.IsPathFullyQualified(value))
		{
			throw new ValidationException($"Layout property value '{nameof(Path)}' is not an absolute path.");
		}
	}
}
