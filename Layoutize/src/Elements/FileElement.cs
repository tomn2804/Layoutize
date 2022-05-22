namespace Layoutize.Elements;

internal sealed class FileElement : ViewElement
{
    public FileElement(FileLayout layout)
        : base(layout)
    {
    }

    public new FileLayout Layout => (FileLayout)base.Layout;
}
