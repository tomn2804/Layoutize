using System.Collections;

namespace Templata;

public sealed partial class BlankTemplate
{
    public new class DetailOption : Template.DetailOption
    {
    }
}

public sealed partial class BlankTemplate : Template<View>
{
    public BlankTemplate(IDictionary details)
        : base(details)
    {
    }

    protected override Context ToBlueprint()
    {
        return new Context.Builder().ToBlueprint();
    }
}
