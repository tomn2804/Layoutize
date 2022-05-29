using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Layoutize.Annotations;
using Layoutize.Contexts;
using Layoutize.Elements;
using Layoutize.Views;

namespace Layoutize.Layouts;

internal class RootDirectoryLayout : DirectoryLayout
{
	[Required]
	[Path]
	public string? Path { get; init; }

	[MemberNotNull(nameof(Path))]
	internal override DirectoryElement CreateElement()
	{
		var element = base.CreateElement();
		Debug.Assert(IsValid());
		return element;
	}

	[MemberNotNull(nameof(Path))]
	internal override DirectoryView CreateView(IBuildContext context)
	{
		Debug.Assert(IsValid());
		var fullName = System.IO.Path.Combine(Path, Name);
		Debug.Assert(FullName.IsValid(fullName));
		return new(new(fullName));
	}

	[MemberNotNull(nameof(Path))]
	internal override void Validate()
	{
		base.Validate();
		Debug.Assert(Path != null);
	}

	[MemberNotNullWhen(true, nameof(Path))]
	internal override bool IsValid()
	{
		var result = base.IsValid();
		if (result) Debug.Assert(Path != null);
		return result;
	}
}
