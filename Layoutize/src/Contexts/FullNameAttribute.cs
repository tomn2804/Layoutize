using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace Layoutize.Contexts;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
internal sealed class FullNameAttribute : LayoutAttribute, IAtom<string>
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
				$"'{nameof(FullNameAttribute)}' value is either null, empty, or consists of only white-space characters."
			);
		}
		if (value.IndexOfAny(Path.GetInvalidPathChars()) != -1)
		{
			throw new ValidationException($"'{nameof(FullNameAttribute)}' value contains invalid characters.");
		}
		if (!Path.IsPathFullyQualified(value))
		{
			throw new ValidationException($"'{nameof(FullNameAttribute)}' value is not an absolute path.");
		}
	}

	public static string? Of(IBuildContext context)
	{
		return Selector<string?>.GetValue(context, typeof(FullNameAttribute), true);
	}

	public bool TryGetValue(IBuildContext context, [NotNullWhen(true)] out string? value)
	{
		if (
			IsDefined(context.Element.Layout.GetType(), typeof(FullNameAttribute))
			&& PathAttribute.Of(context) is string path
			&& NameAttribute.Of(context) is string name
		)
		{
			value = Path.Combine(path, name);
			Debug.Assert(IsValid(value));
			return true;
		}
		value = default;
		return false;
	}
}
