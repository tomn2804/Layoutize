using System.ComponentModel.DataAnnotations;
using Layoutize.Contexts;

namespace Layoutize.Annotations;

public class PathValidationAttribute : ValidationAttribute
{
	public override bool IsValid(object? value)
	{
		return Path.IsValid((string?)value);
	}
}
