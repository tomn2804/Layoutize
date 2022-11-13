using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Layoutize.Annotations;
using Layoutize.Elements;
using Layoutize.Utils;
using Layoutize.Views;
using Path = System.IO.Path;

namespace Layoutize.Layouts;

public class DirectoryLayout : FileSystemLayout
{
	public IEnumerable<Layout> Children { get; init; } = Enumerable.Empty<Layout>();

	internal override DirectoryElement CreateElement()
	{
		Debug.Assert(Model.IsValid(this));
		return new(this);
	}

	internal override IView CreateView(IBuildContext context)
	{
		Debug.Assert(Model.IsValid(this));
		var fullName = FullNameAttribute.Of(context);
		if (fullName == null)
		{
			throw new InvalidOperationException();
		}
		var view = new DirectoryView(new(fullName));
		Debug.Assert(view.Name == Name);
		Debug.Assert(view.FullName == fullName);
		return view;
	}
}
