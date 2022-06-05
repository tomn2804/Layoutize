using System.Diagnostics;
using Layoutize.Elements;

namespace Layoutize.Layouts;

public abstract class StatefulLayout : ComponentLayout
{
	protected internal abstract State CreateState();

	internal sealed override StatefulElement CreateElement(Element? parent)
	{
		Model.Validate(this);
		var element = new StatefulElement(parent, this);
		Debug.Assert(!element.IsMounted);
		return element;
	}
}
