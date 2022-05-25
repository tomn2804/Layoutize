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
				case ViewElement:
					path = FullName.Of(parent);
					return;

				case not null:
					VisitParent(parent);
					return;

				default:
					return;
			}
		}
		VisitParent(context.Element);
		Debug.Assert(TryValidate(path));
		return path;
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
		if (value == null)
		{
			throw new ValidationException($"Attribute value '{nameof(Path)}' is null.");
		}
		if (value.IndexOfAny(System.IO.Path.GetInvalidPathChars()) != -1)
		{
			throw new ValidationException($"Attribute value '{nameof(Path)}' contains invalid characters.");
		}
	}
}
