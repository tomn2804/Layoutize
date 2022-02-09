using System.Collections;

namespace Templatize.Templates;

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
