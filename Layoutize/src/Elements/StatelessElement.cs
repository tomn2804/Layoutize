namespace Layoutize.Elements;

internal sealed class StatelessElement : ComponentElement
{
    internal StatelessElement(StatelessLayout layout)
        : base(layout)
    {
    }

    private new StatelessLayout Layout => (StatelessLayout)base.Layout;

    private protected override sealed Layout Build()
    {
        return Layout.Build(this);
    }
}
