using System.ComponentModel.DataAnnotations;
using System.IO;

namespace Layoutize.Annotations;

internal sealed class NameAttribute : ValidationAttribute
{
	protected override ValidationResult? IsValid(object? value, ValidationContext context)
	{
		if (value is not string name)
		{
			return new($"'{nameof(NameAttribute)}' value must be of type {typeof(string)}.");
		}
		if (string.IsNullOrWhiteSpace(name))
		{
			return new($"'{nameof(NameAttribute)}' value cannot be null, empty, or consists of only white-space characters.");
		}
		if (name.IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
		{
			return new($"'{nameof(NameAttribute)}' value cannot contain invalid characters.");
		}
		return ValidationResult.Success;
	}
}
