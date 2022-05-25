using System.ComponentModel.DataAnnotations;
using Layoutize.Contexts;

namespace Layoutize.Annotations;

public class PathAttribute : ValidationAttribute
{
	protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
	{
		try
		{
			Path.Validate(value?.ToString());
		}
		catch (ValidationException e)
		{
			return new(e.Message);
		}
		return null;
	}
}
