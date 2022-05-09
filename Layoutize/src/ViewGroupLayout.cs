using Layoutize.Elements;
using System.Collections;

namespace Layoutize;

public abstract class ViewGroupLayout : ViewLayout
{
    private protected ViewGroupLayout(IDictionary attributes)
        : base(attributes)
    {
    }

    internal abstract override ViewGroupElement CreateElement();
}
