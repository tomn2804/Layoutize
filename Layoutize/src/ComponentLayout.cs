using Layoutize.Elements;
using System.Collections;

namespace Layoutize;

public abstract class ComponentLayout : Layout
{
    private protected ComponentLayout(IEnumerable attributes)
        : base(attributes)
    {
    }

    internal abstract override ComponentElement CreateElement();
}
