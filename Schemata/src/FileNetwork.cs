using System.Collections.Generic;

namespace Schemata;

public sealed class FileNetwork : Network
{
    public override IEnumerator<Connection> GetEnumerator()
    {
        yield return new Connection(Model);
    }

    internal FileNetwork(FileModel model)
    {
        Model = model;
    }

    protected override FileModel Model { get; }
}
