using System.ComponentModel.DataAnnotations;

namespace Layoutize.Annotations;

public abstract class LayoutAttribute : ValidationAttribute
{
	protected sealed override ValidationResult? IsValid(object? value, ValidationContext context)
	{
		try
		{
			Validate(value);
		}
		catch (ValidationException e)
		{
			return new(e.Message);
		}
		return null;
	}

	protected virtual void Validate(object? value)
	{
	}
}
