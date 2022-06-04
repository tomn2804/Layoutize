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
	[Path]
	public string Path { get; init; } = null!;

	internal override IView CreateView(IBuildContext context)
	{
		Debug.Assert(Model.IsValid(this));
		var view = new DirectoryView(new(System.IO.Path.Combine(Path, Name)));
		Debug.Assert(Contexts.Name.IsValid(view.Name));
		Debug.Assert(FullName.IsValid(view.FullName));
		return view;
	}
}
