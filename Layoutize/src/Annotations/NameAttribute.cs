using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace Layoutize.Annotations;

internal sealed class NameAttribute : LayoutAttribute
{
	protected override void Validate([NotNull] object? value)
	{
		if (value is not string name)
		{
			throw new ValidationException($"'{nameof(NameAttribute)}' value is not of type string.");
		}
		if (string.IsNullOrWhiteSpace(name))
		{
			throw new ValidationException(
				$"'{nameof(NameAttribute)}' value is either null, empty, or consists of only white-space characters."
			);
		}
		if (name.IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
		{
			throw new ValidationException($"'{nameof(NameAttribute)}' value contains invalid characters.");
		}
	}
}
