using System.ComponentModel.DataAnnotations;
using System.IO;

namespace Layoutize.Annotations;

internal sealed class PathAttribute : ValidationAttribute
{
	protected override ValidationResult? IsValid(object? value, ValidationContext context)
	{
		if (value is not string path)
		{
			return new($"'{nameof(PathAttribute)}' value must be of type {typeof(string)}.");
		}
		if (string.IsNullOrWhiteSpace(path))
		{
			return new($"'{nameof(PathAttribute)}' value cannot be null, empty, or consists of only white-space characters.");
		}
		if (path.IndexOfAny(Path.GetInvalidPathChars()) != -1)
		{
			return new($"'{nameof(PathAttribute)}' value cannot contain invalid characters.");
		}
		if (!Path.IsPathFullyQualified(path))
		{
			return new($"'{nameof(PathAttribute)}' value is not an absolute path.");
		}
		return ValidationResult.Success;
	}
}
