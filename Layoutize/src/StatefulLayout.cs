using Layoutize.Elements;

namespace Layoutize;

public abstract class StatefulLayout : ComponentLayout
{
    internal override sealed StatefulElement CreateElement()
    {
        return new(this);
    }

    protected internal abstract State CreateState();
}
