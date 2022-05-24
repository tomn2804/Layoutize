using System.Diagnostics;
using Layoutize.Contexts;
using Layoutize.Elements;
using Layoutize.Views;

namespace Layoutize;

internal class RootDirectoryLayout : DirectoryLayout
{
	public string Path
	{
		get => _path;
		init
		{
			Debug.Assert(Contexts.Path.IsValid(value));
			_path = value;
		}
	}

	internal override DirectoryView CreateView(IBuildContext context)
	{
		var fullName = System.IO.Path.Combine(Path, Name);
		Debug.Assert(FullName.IsValid(fullName));
		return new(new(fullName));
	}

	private readonly string _path = string.Empty;
}
