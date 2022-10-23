using System.Diagnostics;
using Layoutize.Elements;
using Layoutize.Utils;

namespace Layoutize.Layouts;

public abstract class StatefulLayout : ComponentLayout
{
	protected internal abstract State CreateState();

	internal sealed override StatefulElement CreateElement(Element parent)
	{
		Debug.Assert(Model.IsValid(this));
		var element = new StatefulElement(parent, this);
		Debug.Assert(!element.IsMounted);
		Debug.Assert(element.Layout == this);
		Debug.Assert(element.Parent == parent);
		return element;
	}
}
