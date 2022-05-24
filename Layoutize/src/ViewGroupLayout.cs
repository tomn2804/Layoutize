using System.Collections.Generic;
using System.Linq;
using Layoutize.Elements;

namespace Layoutize;

public abstract class ViewGroupLayout : ViewLayout
{
	public IEnumerable<Layout> Children { get; init; } = Enumerable.Empty<Layout>();

	internal abstract override ViewGroupElement CreateElement();
}
