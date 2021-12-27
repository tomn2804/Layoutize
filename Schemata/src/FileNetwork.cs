using System.Collections.Generic;

namespace Schemata;

public sealed class FileNetwork : Network
{
    public override IEnumerator<Node> GetEnumerator()
    {
        yield return new Node(Model);
    }

    internal FileNetwork(FileModel model)
    {
        Model = model;
    }

    protected override FileModel Model { get; }
}
