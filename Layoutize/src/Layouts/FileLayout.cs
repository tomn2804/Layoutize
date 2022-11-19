using System.Diagnostics;
using Layoutize.Annotations;
using Layoutize.Elements;
using Layoutize.Views;

namespace Layoutize.Layouts;

public class FileLayout : FileSystemLayout
{
	internal override FileElement CreateElement()
	{
		Debug.Assert(this.IsValid());
		return new(this);
	}

	internal override IView CreateView(IBuildContext context)
	{
		Debug.Assert(this.IsValid());
		var view = new FileView(new(FullName));
		Debug.Assert(view.Name == Name);
		Debug.Assert(view.FullName == FullName);
		return view;
	}
}
