using System.Collections;

namespace Schemata;

public sealed partial class BlankTemplate
{
    public new class DetailOption : Template.DetailOption
    {
    }
}

public sealed partial class BlankTemplate : Template<Model>
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
