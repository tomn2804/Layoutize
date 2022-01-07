using System.Collections.Generic;

namespace Templata;

public sealed class FileTree : Tree
{
    public override IEnumerator<Node> GetEnumerator()
    {
        Node node = new(View);
        yield return node;
        node.Dispose();
    }

    internal FileTree(FileView view)
    {
        View = view;
    }

    protected override FileView View { get; }
}
