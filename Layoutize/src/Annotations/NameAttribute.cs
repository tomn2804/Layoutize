using Layoutize.Contexts;

namespace Layoutize.Annotations;

internal sealed class NameAttribute : LayoutAttribute
{
	protected override void Validate(object? value)
	{
		Name.Validate((string?)value);
	}
}
