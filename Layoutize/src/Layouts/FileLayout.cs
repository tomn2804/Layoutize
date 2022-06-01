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
		var element = new FileElement(this);
		Debug.Assert(!element.IsMounted);
		return element;
	}

	internal override IView CreateView(IBuildContext context)
	{
		var fullName = Path.Combine(Contexts.Path.Of(context), Name);
		Debug.Assert(FullName.IsValid(fullName));
		return new FileView(new(fullName));
	}
}
