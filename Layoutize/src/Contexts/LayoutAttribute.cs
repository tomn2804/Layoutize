using System;
using System.ComponentModel.DataAnnotations;

namespace Layoutize.Contexts;

[AttributeUsage(AttributeTargets.Property)]
public abstract class LayoutAttribute : ValidationAttribute
{
	protected sealed override ValidationResult? IsValid(object? value, ValidationContext context)
	{
		try
		{
			Validate(value);
			return ValidationResult.Success;
		}
		catch (ValidationException e)
		{
			return new(e.Message);
		}
	}

	protected virtual void Validate(object? value)
	{
	}
}
