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
	public string Path
	{
		get
		{
			Debug.Assert(Contexts.Path.IsValid(_path));
			return _path;
		}
		init => _path = value;
	}

	internal override IView CreateView(IBuildContext context)
	{
		Debug.Assert(IsValid());
		var fullName = System.IO.Path.Combine(Path, Name);
		Debug.Assert(FullName.IsValid(fullName));
		return new DirectoryView(new(fullName));
	}

	private readonly string? _path;
}
