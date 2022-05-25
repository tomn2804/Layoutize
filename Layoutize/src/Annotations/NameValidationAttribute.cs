using System.ComponentModel.DataAnnotations;
using Layoutize.Contexts;

namespace Layoutize.Annotations;

public class NameValidationAttribute : ValidationAttribute
{
	public override bool IsValid(object? value)
	{
		return Name.IsValid((string?)value);
	}
}
