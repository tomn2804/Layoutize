using Layoutize.Elements;

namespace Layoutize;

public abstract class StatefulLayout : ComponentLayout
{
	protected internal abstract State CreateState();

	internal sealed override StatefulElement CreateElement()
	{
		return new(this);
	}
}
