using Layoutize.Elements;

namespace Layoutize;

public abstract class StatefulLayout<T> : ComponentLayout where T : StatefulLayout<T>
{
    internal override sealed StatefulElement<T> CreateElement()
    {
        return new(this);
    }

    protected internal abstract State<T> CreateState();
}
