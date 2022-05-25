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
		return new(this);
	}

	internal override FileView CreateView(IBuildContext context)
	{
		var fullName = Path.Combine(Contexts.Path.Of(context), Name);
		Debug.Assert(FullName.IsValid(fullName));
		return new(new(fullName));
	}
}
