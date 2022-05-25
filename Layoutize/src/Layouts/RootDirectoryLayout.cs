using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Layoutize.Annotations;
using Layoutize.Contexts;
using Layoutize.Elements;
using Layoutize.Views;

namespace Layoutize.Layouts;

internal class RootDirectoryLayout : DirectoryLayout
{
	[Required]
	[PathValidation]
	public string Path { get; init; } = null!;

	internal override DirectoryView CreateView(IBuildContext context)
	{
		var fullName = System.IO.Path.Combine(Path, Name);
		Debug.Assert(FullName.IsValid(fullName));
		return new(new(fullName));
	}
}
