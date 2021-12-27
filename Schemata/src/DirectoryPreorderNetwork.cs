using System.Collections.Generic;

namespace Schemata;

public sealed class DirectoryPreorderNetwork : Network
{
    public override IEnumerator<Connection> GetEnumerator()
    {
        Connection parentConnection = new(Model);
        yield return parentConnection;
        foreach (Model child in Model.Children)
        {
            foreach (Connection childConnection in child.Network)
            {
                yield return childConnection;
            }
        }
        parentConnection.Dispose();
    }

    internal DirectoryPreorderNetwork(DirectoryModel model)
    {
        Model = model;
    }

    protected override DirectoryModel Model { get; }
}
