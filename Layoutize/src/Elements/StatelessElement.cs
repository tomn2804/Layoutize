namespace Layoutize.Elements;

internal sealed class StatelessElement : ComponentElement
{
    public StatelessElement(StatelessLayout layout)
        : base(layout)
    {
    }

    private new StatelessLayout Layout => (StatelessLayout)base.Layout;

    protected override sealed Layout Build()
    {
        return Layout.Build(this);
    }
}
