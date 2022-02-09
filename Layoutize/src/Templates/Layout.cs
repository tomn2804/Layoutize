using System.Collections.Generic;

namespace Layoutize.Templates;

public sealed partial class Layout
{
    public IReadOnlyDictionary<object, object> Attributes { get; init; } = null!;

    public Builder ToBuilder()
    {
        return new(this);
    }

    private Layout()
    {
    }

    private IReadOnlyList<Template> Templates { get; init; } = null!;
}
