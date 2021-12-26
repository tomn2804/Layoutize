using System.Collections.Generic;

namespace Schemata;

public sealed class FileNetwork : Network
{
    public override IEnumerator<Connection> GetEnumerator()
    {
        return new FileEnumerator(this);
    }

    internal FileNetwork(FileModel model)
    {
        Model = model;
    }

    internal override FileModel Model { get; }
}
