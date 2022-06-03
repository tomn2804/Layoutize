using System.Diagnostics;
using Layoutize.Elements;

namespace Layoutize.Layouts;

public abstract class StatelessLayout : ComponentLayout
{
	protected internal abstract Layout Build(IBuildContext context);

	internal sealed override StatelessElement CreateElement()
	{
		Model.Validate(this);
		var element = new StatelessElement(this);
		Debug.Assert(!element.IsMounted);
		return element;
	}
}
