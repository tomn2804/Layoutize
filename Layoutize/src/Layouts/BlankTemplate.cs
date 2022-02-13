using System.Collections;

namespace Layoutize.Layouts;

public sealed partial class BlankTemplate
{
    public new class DetailOption : Layout.DetailOption
    {
    }
}

public sealed partial class BlankTemplate : Layout
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
