using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using Layoutize.Annotations;
using Layoutize.Contexts;
using Layoutize.Elements;
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
	public string Name { get; init; } = null!;

	internal override DirectoryElement CreateElement()
	{
		Model.Validate(this);
		var element = new DirectoryElement(this);
		Debug.Assert(!element.IsMounted);
		return element;
	}

	internal override IView CreateView(IBuildContext context)
	{
		Debug.Assert(Model.IsValid(this));
		var view = new DirectoryView(new(Path.Combine(Contexts.Path.Of(context), Name)));
		Debug.Assert(Contexts.Name.IsValid(view.Name));
		Debug.Assert(FullName.IsValid(view.FullName));
		return view;
	}
}
