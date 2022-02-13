using System.Collections.Generic;

namespace Layoutize.Layouts;

public class Element
{
    public IReadOnlyDictionary<object, object> Attributes { get; init; } = null!;

    public Builder ToBuilder()
    {
        return new(this);
    }

    public Element()
    {
    }

    private IReadOnlyList<Layout> Templates { get; init; } = null!;
}
