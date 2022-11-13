using Layoutize.Elements;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace Layoutize.Contexts;

[AttributeUsage(AttributeTargets.Class, Inherited = true)]
internal class PathAttribute : LayoutAttribute, IAtom<string>
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

	protected override void Validate([NotNull] object? value)
	{
		Validate((string?)value);
	}

	internal static void Validate([NotNull] string? value)
	{
		if (string.IsNullOrWhiteSpace(value))
		{
			throw new ValidationException(
				$"'{nameof(PathAttribute)}' value is either null, empty, or consists of only white-space characters."
			);
		}
		if (value.IndexOfAny(Path.GetInvalidPathChars()) != -1)
		{
			throw new ValidationException($"'{nameof(PathAttribute)}' value contains invalid characters.");
		}
		if (!Path.IsPathFullyQualified(value))
		{
			throw new ValidationException($"'{nameof(PathAttribute)}' value is not an absolute path.");
		}
	}

	public static string? Of(IBuildContext context)
	{
		return Selector<string?>.GetValue(context, typeof(PathAttribute));
	}

	public bool TryGetValue(IBuildContext context, [NotNullWhen(true)] out string? value)
	{
		if (context.Element is FileSystemElement element)
		{
			value = element.View?.FullName;
			Debug.Assert(IsValid(value));
			return true;
		}
		value = default;
		return false;
	}
}
