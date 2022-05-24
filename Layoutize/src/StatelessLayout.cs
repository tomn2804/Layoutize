using Layoutize.Contexts;
using Layoutize.Elements;

namespace Layoutize;

public abstract class StatelessLayout : ComponentLayout
{
	protected internal abstract Layout Build(IBuildContext context);

	internal sealed override StatelessElement CreateElement()
	{
		return new(this);
	}
}
