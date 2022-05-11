using Layoutize.Elements;
using System.Collections;

namespace Layoutize;

public abstract class StatelessLayout : ComponentLayout
{
    protected StatelessLayout(IDictionary attributes)
        : base(attributes)
    {
    }

    internal override sealed StatelessElement CreateElement()
    {
        return new(this);
    }

    protected internal abstract Layout Build(IBuildContext context);
}
