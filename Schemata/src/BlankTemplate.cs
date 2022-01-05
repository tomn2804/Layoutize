using System;
using System.Collections;

namespace Schemata;

public sealed class BlankTemplate : Template<Model>
{
    public BlankTemplate(IDictionary details)
        : base(details)
    {
    }

    protected override Blueprint ToBlueprint()
    {
        return new Blueprint.Builder().ToBlueprint();
    }
}
