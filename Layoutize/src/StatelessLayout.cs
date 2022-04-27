using Layoutize.Elements;
using System.Collections;

namespace Layoutize;

public abstract class StatelessLayout : ComponentLayout
{
    internal override sealed StatelessElement CreateElement()
    {
        return new(this);
    }

    protected internal abstract Layout Build(IBuildContext context);

    protected StatelessLayout(IDictionary attributes)
        : base(attributes)
    {
    }
}
