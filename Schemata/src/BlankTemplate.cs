using System;
using System.Collections;

namespace Schemata;

public sealed class BlankTemplate : Template<Model>
{
    public BlankTemplate(IEnumerable details)
        : base(details)
    {
    }

    protected override Blueprint ToBlueprint()
    {
        return base.ToBlueprint();
    }
}
