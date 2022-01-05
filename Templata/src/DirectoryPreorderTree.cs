using System.Collections.Generic;

namespace Templata;

public sealed class DirectoryPreorderTree : Tree
{
    public override IEnumerator<Node> GetEnumerator()
    {
        Node parentNode = new(Model);
        yield return parentNode;
        foreach (Model child in Model.Children)
        {
            foreach (Node childNode in child.Tree)
            {
                yield return childNode;
            }
        }
        parentNode.Dispose();
    }

    internal DirectoryPreorderTree(DirectoryModel model)
    {
        Model = model;
    }

    protected override DirectoryModel Model { get; }
}
