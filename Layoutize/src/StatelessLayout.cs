using Layoutize.Elements;

namespace Layoutize;

public abstract class StatelessLayout : ComponentLayout
{
    internal override sealed StatelessElement CreateElement()
    {
        return new(this);
    }

    protected internal abstract Layout Build(IBuildContext context);
}
