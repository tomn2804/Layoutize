using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Layoutize.Annotations;
using Layoutize.Contexts;
using Layoutize.Elements;
using Layoutize.Views;
using Path = System.IO.Path;

namespace Layoutize.Layouts;

public class FileLayout : ViewLayout
{
	[Required]
	[Name]
	public string Name { get; init; } = null!;

	internal override FileElement CreateElement(Element parent)
	{
		Model.Validate(this);
		var element = new FileElement(parent, this);
		Debug.Assert(Model.IsValid(this));
		Debug.Assert(!element.IsMounted);
		return element;
	}

	internal override IView CreateView(IBuildContext context)
	{
		Debug.Assert(Model.IsValid(this));
		var view = new FileView(new(Path.Combine(Contexts.Path.Of(context), Name)));
		Debug.Assert(Model.IsValid(this));
		Debug.Assert(Contexts.Name.IsValid(view.Name));
		Debug.Assert(FullName.IsValid(view.FullName));
		return view;
	}
}
