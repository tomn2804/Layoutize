using System;
using System.Diagnostics;
using System.ComponentModel.DataAnnotations;
using Layoutize.Views;
using Layoutize.Utils;
using Layoutize.Elements;
using Layoutize.Contexts;
using Layoutize.Annotations;

namespace Layoutize.Layouts;

internal sealed class RootDirectoryLayout : ViewGroupLayout
{
	[Required]
	[FullName]
	public string FullName
	{
		get
		{
			Debug.Assert(Validator.TryValidateProperty(_fullName, new(this) { MemberName = nameof(FullName) }, null));
			return _fullName!;
		}
		init
		{
			Validator.ValidateProperty(value, new(this) { MemberName = nameof(FullName) });
			_fullName = value;
			Debug.Assert(_fullName == value);
		}
	}

	internal RootDirectoryElement CreateElement()
	{
		Debug.Assert(Model.IsValid(this));
		var element = new RootDirectoryElement(this);
		Debug.Assert(!element.IsMounted);
		Debug.Assert(element.Layout == this);
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
		Debug.Assert(view.FullName == FullName);
		return view;
	}

	private string? _fullName;
}
