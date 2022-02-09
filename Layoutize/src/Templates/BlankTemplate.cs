using System.Collections;

namespace Layoutize.Templates;

public sealed partial class BlankTemplate
{
    public new class DetailOption : Template.DetailOption
    {
    }
}

public sealed partial class BlankTemplate : Template
{
    public BlankTemplate(IDictionary attributes)
        : base(attributes)
    {
    }

    protected override View Build()
    {
        return base.Build();
    }
}
