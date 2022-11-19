using System.Diagnostics;
using Layoutize.Elements;
using Layoutize.Annotations;

namespace Layoutize.Layouts;

public abstract class StatelessLayout : ComponentLayout
{
	protected internal abstract Layout Build(IBuildContext context);

	internal sealed override StatelessElement CreateElement()
	{
		Debug.Assert(this.IsValid());
		return new(this);
	}
}
