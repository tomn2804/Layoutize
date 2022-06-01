using System.Diagnostics.CodeAnalysis;
using Layoutize.Contexts;

namespace Layoutize.Annotations;

public sealed class NameAttribute : LayoutAttribute
{
	protected override void Validate([NotNull] object? value)
	{
		Name.Validate((string?)value);
	}
}
