using Layoutize.Elements;
using System.Collections;

namespace Layoutize;

public abstract class StatefulLayout : ComponentLayout
{
    internal override sealed StatefulElement CreateElement()
    {
        return new(this);
    }

    protected internal abstract State CreateState();

    protected StatefulLayout(IDictionary attributes)
        : base(attributes)
    {
    }
}
