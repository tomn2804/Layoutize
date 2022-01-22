using System.Collections.Generic;

namespace Templatize.Templates;

public partial class Layout
{
    public IReadOnlyDictionary<object, object> Properties { get; init; } = null!;

    public Builder ToBuilder()
    {
        return new(this);
    }

    private Layout()
    {
    }

    private IReadOnlyList<Template> Templates { get; init; } = null!;
}
