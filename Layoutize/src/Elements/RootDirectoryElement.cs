using System;
using System.Diagnostics;
using Layoutize.Layouts;

namespace Layoutize.Elements;

internal sealed class RootDirectoryElement : ViewGroupElement
{
	public RootDirectoryElement(RootDirectoryLayout layout)
		: base(layout)
	{
	}

	public void Mount()
	{
		Mount(null);
	}

	public new void Mount(Element? parent)
	{
		Debug.Assert(parent == null);
		base.Mount(parent);
	}

	public override string Name => throw new InvalidOperationException();
}
