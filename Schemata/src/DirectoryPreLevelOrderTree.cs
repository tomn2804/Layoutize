using System.Collections.Generic;
using System.Linq;

namespace Schemata;

public sealed class DirectoryPreLevelOrderTree : Tree
{
    public override IEnumerator<Node> GetEnumerator()
    {
        Node parentNode = new(Model);
        yield return parentNode;
        Queue<IEnumerator<Node>> childNodes = new(Model.Children.Count);
        foreach (Model child in Model.Children)
        {
            IEnumerator<Node> childNode = child.Tree.GetEnumerator();
            if (childNode.MoveNext())
            {
                childNodes.Enqueue(childNode);
                yield return childNode.Current;
            }
        }
        while (childNodes.Any())
        {
            IEnumerator<Node> childNode = childNodes.Dequeue();
            while (childNode.MoveNext())
            {
                yield return childNode.Current;
            }
        }
        parentNode.Dispose();
    }

    internal DirectoryPreLevelOrderTree(DirectoryModel model)
    {
        Model = model;
    }

    protected override DirectoryModel Model { get; }
}
