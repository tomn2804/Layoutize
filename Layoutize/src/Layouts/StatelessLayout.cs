using System.Diagnostics;
using Layoutize.Annotations;
using Layoutize.Elements;
using Layoutize.Utils;

namespace Layoutize.Layouts;

public abstract class StatelessLayout : ComponentLayout
{
	protected internal abstract Layout Build(IBuildContext context);

	internal sealed override StatelessElement CreateElement()
	{
		Debug.Assert(Model.IsValid(this));
		return new(this);
	}
}
