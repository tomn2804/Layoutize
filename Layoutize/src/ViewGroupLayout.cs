using Layoutize.Elements;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Layoutize;

public abstract class ViewGroupLayout : ViewLayout
{
    private protected ViewGroupLayout(IDictionary attributes)
        : base(attributes)
    {
        Debug.Assert(Children != null);
    }

    public IEnumerable<Layout> Children { get; }

    internal abstract override ViewGroupElement CreateElement();
}
