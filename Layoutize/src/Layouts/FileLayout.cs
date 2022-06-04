using System.Diagnostics;
using Layoutize.Contexts;
using Layoutize.Elements;
using Layoutize.Views;
using Path = System.IO.Path;

namespace Layoutize.Layouts;

public class FileLayout : ViewLayout
{
	internal override FileElement CreateElement()
	{
		Model.Validate(this);
		var element = new FileElement(this);
		Debug.Assert(!element.IsMounted);
		return element;
	}

	internal override IView CreateView(IBuildContext context)
	{
		Debug.Assert(Model.IsValid(this));
		var view = new FileView(new(Path.Combine(Contexts.Path.Of(context), Name)));
		Debug.Assert(Contexts.Name.IsValid(view.Name));
		Debug.Assert(FullName.IsValid(view.FullName));
		return view;
	}
}
