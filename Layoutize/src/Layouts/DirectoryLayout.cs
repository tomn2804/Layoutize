using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Layoutize.Annotations;
using Layoutize.Elements;
using Layoutize.Views;

namespace Layoutize.Layouts;

public class DirectoryLayout : FileSystemLayout
{
	public IEnumerable<Layout> Children { get; init; } = Enumerable.Empty<Layout>();

	internal override DirectoryElement CreateElement()
	{
		Debug.Assert(this.IsValid());
		return new(this);
	}

	internal override IView CreateView(IBuildContext context)
	{
		Debug.Assert(this.IsValid());
		var view = new DirectoryView(new(FullName));
		Debug.Assert(view.Name == Name);
		Debug.Assert(view.FullName == FullName);
		return view;
	}
}
