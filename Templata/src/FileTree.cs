using System.Collections.Generic;

namespace Templata;

public sealed class FileTree : Tree
{
    public override IEnumerator<Node> GetEnumerator()
    {
        Node node = new(Model);
        yield return node;
        node.Dispose();
    }

    internal FileTree(FileModel model)
    {
        Model = model;
    }

    protected override FileModel Model { get; }
}
