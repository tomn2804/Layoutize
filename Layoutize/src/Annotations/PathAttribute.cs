using Layoutize.Contexts;

namespace Layoutize.Annotations;

internal sealed class PathAttribute : LayoutAttribute
{
	protected override void Validate(object? value)
	{
		Path.Validate((string?)value);
	}
}
