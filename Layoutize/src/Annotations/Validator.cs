using System;
using System.Diagnostics.CodeAnalysis;

namespace Layoutize.Annotations;

internal static class Validator
{
	public static bool IsValid(this object instance)
	{
		return System.ComponentModel.DataAnnotations.Validator.TryValidateObject(instance, new(instance), null, true);
	}

	public static bool IsMemberValid(this object instance, string memberName, [NotNullWhen(true)] object? value)
	{
		return value != null && System.ComponentModel.DataAnnotations.Validator.TryValidateObject(value, new(instance) { MemberName = memberName }, null);
	}

	public static void Validate(this object instance)
	{
		System.ComponentModel.DataAnnotations.Validator.ValidateObject(instance, new(instance));
	}

	public static void ValidateMember(this object instance, string memberName, [NotNull] object? value)
	{
		if (value == null) throw new ArgumentNullException(nameof(value));
		System.ComponentModel.DataAnnotations.Validator.ValidateObject(value, new(instance) { MemberName = memberName });
	}
}
