namespace Layoutize.Elements;

internal sealed class StatelessElement : ComponentElement
{
    internal StatelessElement(StatelessLayout layout)
        : base(layout)
    {
    }

    private StatelessLayout StatelessLayout => (StatelessLayout)Layout;

    private protected override sealed Layout Build()
    {
        return StatelessLayout.Build(this);
    }
}
