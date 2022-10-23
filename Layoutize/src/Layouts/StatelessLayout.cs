using System.Diagnostics;
using Layoutize.Contexts;
using Layoutize.Elements;
using Layoutize.Utils;

namespace Layoutize.Layouts;

public abstract class StatelessLayout : ComponentLayout
{
	protected internal abstract Layout Build(IBuildContext context);

	internal sealed override StatelessElement CreateElement(Element parent)
	{
		Debug.Assert(Model.IsValid(this));
		var element = new StatelessElement(parent, this);
		Debug.Assert(!element.IsMounted);
		Debug.Assert(element.Layout == this);
		Debug.Assert(element.Parent == parent);
		return element;
	}
}
