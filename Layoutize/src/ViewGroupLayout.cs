using Layoutize.Elements;
using System.Collections;

namespace Layoutize;

public abstract class ViewGroupLayout : ViewLayout
{
    internal abstract override ViewGroupElement CreateElement();

    private protected ViewGroupLayout(IDictionary attributes)
           : base(attributes)
    {
    }
}
