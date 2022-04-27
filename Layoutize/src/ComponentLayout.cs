using Layoutize.Elements;
using System.Collections;

namespace Layoutize;

public abstract class ComponentLayout : Layout
{
    internal abstract override ComponentElement CreateElement();

    private protected ComponentLayout(IDictionary attributes)
        : base(attributes)
    {
    }
}
