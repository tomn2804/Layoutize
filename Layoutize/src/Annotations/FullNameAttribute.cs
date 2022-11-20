using System.ComponentModel.DataAnnotations;
using System.IO;

namespace Layoutize.Annotations;

internal sealed class FullNameAttribute : ValidationAttribute
{
	protected override ValidationResult? IsValid(object? value, ValidationContext context)
	{
		if (value is not string fullName)
		{
			return new($"'{nameof(FullNameAttribute)}' value must be of type {typeof(string)}.");
		}
		if (string.IsNullOrWhiteSpace(fullName))
		{
			return new($"'{nameof(FullNameAttribute)}' value cannot be null, empty, or consists of only white-space characters.");
		}
		if (fullName.IndexOfAny(Path.GetInvalidPathChars()) != -1)
		{
			return new($"'{nameof(FullNameAttribute)}' value cannot contain invalid characters.");
		}
		if (!Path.IsPathFullyQualified(fullName))
		{
			return new($"'{nameof(FullNameAttribute)}' value is not an absolute path.");
		}
		return ValidationResult.Success;
	}
}
