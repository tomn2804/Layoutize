using Layoutize.Contexts;

namespace Layoutize.Annotations;

public sealed class NameAttribute : LayoutAttribute
{
	protected override void Validate(object? value)
	{
		Name.Validate((string?)value);
	}
}
