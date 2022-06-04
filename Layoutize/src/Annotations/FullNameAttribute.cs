using System.Diagnostics.CodeAnalysis;
using Layoutize.Contexts;

namespace Layoutize.Annotations;

internal sealed class FullNameAttribute : LayoutAttribute
{
	protected override void Validate([NotNull] object? value)
	{
		FullName.Validate((string?)value);
	}
}
