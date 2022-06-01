using System.Diagnostics.CodeAnalysis;
using Layoutize.Contexts;

namespace Layoutize.Annotations;

internal sealed class PathAttribute : LayoutAttribute
{
	protected override void Validate([NotNull] object? value)
	{
		Path.Validate((string?)value);
	}
}
