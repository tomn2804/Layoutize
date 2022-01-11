using System.Collections;

namespace Templatize;

public sealed partial class BlankTemplate
{
    public new class DetailOption : Template.DetailOption
    {
    }
}

public sealed partial class BlankTemplate : Template
{
    public BlankTemplate(IDictionary details)
        : base(details)
    {
    }

    protected override View Build()
    {
        return base.Build();
    }
}
