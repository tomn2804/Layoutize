using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using Layoutize.Annotations;
using Layoutize.Contexts;
using Layoutize.Elements;
using Layoutize.Utils;
using Layoutize.Views;
using Path = System.IO.Path;

namespace Layoutize.Layouts;

public class DirectoryLayout : ViewGroupLayout
{
	public new object Children
	{
		get => base.Children;
		init => base.Children = value switch
		{
			IEnumerable<object> children => children.Cast<Layout>(),
			_ => new[] { (Layout)value },
		};
	}

	[Required]
	[Name]
	public string Name
	{
		get
		{
			Debug.Assert(Validator.TryValidateProperty(_name, new(this) { MemberName = nameof(Name) }, null));
			return _name!;
		}
		init
		{
			Validator.ValidateProperty(value, new(this) { MemberName = nameof(Name) });
			_name = value;
			Debug.Assert(_name == value);
		}
	}

	internal override DirectoryElement CreateElement(Element parent)
	{
		Debug.Assert(Model.IsValid(this));
		var element = new DirectoryElement(parent, this);
		Debug.Assert(!element.IsMounted);
		Debug.Assert(element.Layout == this);
		Debug.Assert(element.Parent == parent);
		return element;
	}

	internal override IView CreateView(IBuildContext context)
	{
		Debug.Assert(Model.IsValid(this));
		var view = new DirectoryView(new(Path.Combine(Contexts.Path.Of(context), Name)));
		Debug.Assert(view.Name == Name);
		Debug.Assert(view.FullName == FullName.Of(context));
		return view;
	}

	private string? _name;
}
