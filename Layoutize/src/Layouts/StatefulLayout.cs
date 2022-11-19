using System.Diagnostics;
using Layoutize.Elements;
using Layoutize.Annotations;

namespace Layoutize.Layouts;

public abstract class StatefulLayout : ComponentLayout
{
	protected internal abstract State CreateState();

	internal sealed override StatefulElement CreateElement()
	{
		Debug.Assert(this.IsValid());
		return new(this);
	}
}
