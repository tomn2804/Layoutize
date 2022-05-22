namespace Layoutize.Elements;

internal sealed class DirectoryElement : ViewGroupElement
{
    public DirectoryElement(DirectoryLayout layout)
        : base(layout)
    {
    }

    public new DirectoryLayout Layout => (DirectoryLayout)base.Layout;
}
