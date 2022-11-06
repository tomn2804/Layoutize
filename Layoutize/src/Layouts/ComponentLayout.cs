using Layoutize.Elements;

namespace Layoutize.Layouts;

public abstract class ComponentLayout : Layout
{
	internal abstract override ComponentElement CreateElement();
}
