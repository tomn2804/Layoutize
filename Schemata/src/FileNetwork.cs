using System.Collections.Generic;

namespace Schemata;

public class FileNetwork : Network
{
    public FileNetwork(FileModel model)
    {
        Model = model;
    }

    public override FileModel Model { get; }

    public override IEnumerator<Connection> GetEnumerator()
    {
        return new FileEnumerator(this);
    }
}
