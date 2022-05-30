using System;
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
		get => _path ?? throw new InvalidOperationException($"Attribute {nameof(Path)} is uninitialized.");
		init => _path = value;
	}

	internal override DirectoryView CreateView(IBuildContext context)
	{
		var fullName = System.IO.Path.Combine(Path, Name);
		Debug.Assert(FullName.IsValid(fullName));
		return new(new(fullName));
	}

	private readonly string? _path;
}
