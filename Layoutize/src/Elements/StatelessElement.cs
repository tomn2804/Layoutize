using System.Diagnostics;

namespace Layoutize.Elements;

internal sealed class StatelessElement : ComponentElement
{
    internal StatelessElement(StatelessLayout layout)
        : base(layout)
    {
    }

    private protected override sealed Layout Build()
    {
        Debug.Assert(!IsDisposed);
        return StatelessLayout.Build(this);
    }

    private StatelessLayout StatelessLayout => (StatelessLayout)Layout;
}
