using System.Collections.Generic;

namespace Schemata;

public sealed class FileTree : Tree
{
    public override IEnumerator<Node> GetEnumerator()
    {
        yield return new Node(Model);
    }

    internal FileTree(FileModel model)
    {
        Model = model;
    }

    protected override FileModel Model { get; }
}
