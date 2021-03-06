using System;
using Layoutize.Layouts;

namespace Layoutize.Elements;

internal sealed class RootDirectoryElement : ViewGroupElement
{
	public RootDirectoryElement(RootDirectoryLayout layout)
		: base(null!, layout)
	{
	}

	public override Element Parent => throw new InvalidOperationException();
}
