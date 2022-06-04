using System;
using System.Diagnostics;

namespace Layoutize.Elements;

public sealed class ElementNotMountedException : ApplicationException
{
	internal ElementNotMountedException(Element element)
	{
		Debug.Assert(!element.IsMounted);
	}
}
