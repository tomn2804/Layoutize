using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

namespace Layoutize.Contexts;

public sealed class NameAttribute : LayoutAttribute, IAtom<string>
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
				$"'{nameof(NameAttribute)}' value is either null, empty, or consists of only white-space characters."
			);
		}
		if (value.IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
		{
			throw new ValidationException($"'{nameof(NameAttribute)}' value contains invalid characters.");
		}
	}

	public static string? Of(IBuildContext context)
	{
		return Selector<string?>.GetValue(context, typeof(NameAttribute), true);
	}

	public bool TryGetValue(IBuildContext context, [NotNullWhen(true)] out string? value)
	{
		var layout = context.Element.Layout;
		var property = layout.GetType().GetProperties().FirstOrDefault(property => IsDefined(property, GetType()));
		if (property != null)
		{
			value = (string?)property.GetValue(layout);
			Debug.Assert(IsValid(value));
			return true;
		}
		value = default;
		return false;
	}
}
