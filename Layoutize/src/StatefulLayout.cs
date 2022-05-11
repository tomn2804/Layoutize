using Layoutize.Elements;
using System.Collections;

namespace Layoutize;

public abstract class StatefulLayout : ComponentLayout
{
    protected StatefulLayout(IEnumerable attributes)
        : base(attributes)
    {
    }

    internal override sealed StatefulElement CreateElement()
    {
        return new(this);
    }

    protected internal abstract State CreateState();
}
