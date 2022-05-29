using Layoutize.Elements;

namespace Layoutize.Layouts;

public abstract class StatefulLayout : ComponentLayout
{
	protected internal abstract State CreateState();

	internal sealed override StatefulElement CreateElement()
	{
		Validate();
		return new(this);
	}
}
