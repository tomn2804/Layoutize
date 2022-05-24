using Layoutize.Elements;

namespace Layoutize;

public abstract class ComponentLayout : Layout
{
	internal abstract override ComponentElement CreateElement();
}
