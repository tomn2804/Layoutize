namespace Layoutize.Elements;

internal sealed class StatelessElement : ComponentElement
{
    public StatelessElement(StatelessLayout layout)
        : base(layout)
    {
    }

    public new StatelessLayout Layout => (StatelessLayout)base.Layout;

    protected override sealed Layout Build()
    {
        return Layout.Build(this);
    }
}
