using System;
using System.Diagnostics;

namespace Layoutize.Elements;

internal sealed class ElementNotMountedException : ApplicationException
{
	public ElementNotMountedException(Element element)
	{
		Debug.Assert(!element.IsMounted);
	}
}
