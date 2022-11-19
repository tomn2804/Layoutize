using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace Layoutize.Annotations;

internal sealed class FullNameAttribute : LayoutAttribute
{
	protected override void Validate([NotNull] object? value)
	{
		if (value is not string fullName)
		{
			throw new ValidationException($"'{nameof(FullNameAttribute)}' value is not of type string.");
		}
		if (string.IsNullOrWhiteSpace(fullName))
		{
			throw new ValidationException(
				$"'{nameof(FullNameAttribute)}' value is either null, empty, or consists of only white-space characters."
			);
		}
		if (fullName.IndexOfAny(Path.GetInvalidPathChars()) != -1)
		{
			throw new ValidationException($"'{nameof(FullNameAttribute)}' value contains invalid characters.");
		}
		if (!Path.IsPathFullyQualified(fullName))
		{
			throw new ValidationException($"'{nameof(FullNameAttribute)}' value is not an absolute path.");
		}
	}
}
