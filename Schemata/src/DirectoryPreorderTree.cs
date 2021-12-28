using System.Collections.Generic;

namespace Schemata;

public sealed class DirectoryPreorderTree : Tree
{
    public override IEnumerator<Node> GetEnumerator()
    {
        Node parentConnection = new(Model);
        yield return parentConnection;
        foreach (Model child in Model.Children)
        {
            foreach (Node childConnection in child.Tree)
            {
                yield return childConnection;
            }
        }
        parentConnection.Dispose();
    }

    internal DirectoryPreorderTree(DirectoryModel model)
    {
        Model = model;
    }

    protected override DirectoryModel Model { get; }
}
