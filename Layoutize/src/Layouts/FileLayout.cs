using System.Diagnostics;
using Layoutize.Contexts;
using Layoutize.Elements;
using Layoutize.Utils;
using Layoutize.Views;
using Path = System.IO.Path;

namespace Layoutize.Layouts;

public class FileLayout : FileSystemLayout
{
	internal override FileElement CreateElement()
	{
		Debug.Assert(Model.IsValid(this));
		return new(this);
	}

	internal override IView CreateView(IBuildContext context)
	{
		Debug.Assert(Model.IsValid(this));
		var view = new FileView(new(Path.Combine(Contexts.Path.Of(context), Name)));
		Debug.Assert(view.Name == Name);
		Debug.Assert(view.FullName == FullName.Of(context));
		return view;
	}
}
