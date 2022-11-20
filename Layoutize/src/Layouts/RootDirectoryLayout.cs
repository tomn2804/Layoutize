using System;
using System.Diagnostics;
using Layoutize.Views;
using Layoutize.Elements;
using Layoutize.Annotations;

namespace Layoutize.Layouts;

internal sealed class RootDirectoryLayout : DirectoryLayout
{
	public override string Name => throw new NotSupportedException(); // TODO: Validator will trip this

	public RootDirectoryLayout(string fullName)
	{
		Debug.Assert(this.IsMemberValid(nameof(FullName), fullName));
		_fullName = fullName;
		Debug.Assert(FullName == fullName);
	}

	public override string FullName
	{
		get
		{
			Debug.Assert(this.IsMemberValid(nameof(FullName), _fullName));
			return _fullName;
		}
	}

	internal override RootDirectoryElement CreateElement()
	{
		Debug.Assert(this.IsValid());
		return new(this);
	}

	internal override IView CreateView(IBuildContext context)
	{
		Debug.Assert(this.IsValid());
		var view = new RootDirectoryView(new(FullName));
		Debug.Assert(view.FullName == FullName);
		return view;
	}

	private readonly string? _fullName;
}
