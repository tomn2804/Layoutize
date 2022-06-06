using System.Diagnostics;
using Layoutize.Elements;

namespace Layoutize.Layouts;

public abstract class StatelessLayout : ComponentLayout
{
	protected internal abstract Layout Build(IBuildContext context);

	internal sealed override StatelessElement CreateElement(Element parent)
	{
		Model.Validate(this);
		var element = new StatelessElement(parent, this);
		Debug.Assert(!element.IsMounted);
		Debug.Assert(element.Parent == parent);
		return element;
	}
}
