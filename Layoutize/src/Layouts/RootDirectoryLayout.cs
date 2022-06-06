using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Layoutize.Annotations;
using Layoutize.Contexts;
using Layoutize.Elements;
using Layoutize.Views;

namespace Layoutize.Layouts;

internal class RootDirectoryLayout : ViewGroupLayout
{
	[Required]
	[FullName]
	public string FullName { get; init; } = null!;

	internal RootDirectoryElement CreateElement()
	{
		Debug.Assert(Model.IsValid(this));
		var element = new RootDirectoryElement(this);
		Debug.Assert(!element.IsMounted);
		return element;
	}

	internal override RootDirectoryElement CreateElement(Element parent)
	{
		throw new InvalidOperationException();
	}

	internal override IView CreateView(IBuildContext context)
	{
		Debug.Assert(Model.IsValid(this));
		var view = new RootDirectoryView(new(FullName));
		Debug.Assert(Contexts.FullName.IsValid(view.FullName));
		return view;
	}
}
