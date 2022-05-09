using Layoutize.Elements;
using System.Collections;

namespace Layoutize;

public abstract class ComponentLayout : Layout
{
    private protected ComponentLayout(IDictionary attributes)
        : base(attributes)
    {
    }

    internal abstract override ComponentElement CreateElement();
}
