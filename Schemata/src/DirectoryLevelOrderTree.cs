using System;
using System.Collections.Generic;
using System.Linq;

namespace Schemata;

public sealed class DirectoryLevelOrderTree : Tree
{
    public override IEnumerator<Node> GetEnumerator()
    {
        Node parentConnection = new(Model);
        yield return parentConnection;
        Queue<IEnumerator<Node>> childrenConnections = new(Model.Children.Count);
        foreach (Model child in Model.Children)
        {
            IEnumerator<Node> enumerator = child.Tree.GetEnumerator();
            if (enumerator.MoveNext())
            {
                childrenConnections.Enqueue(enumerator);
                yield return enumerator.Current;
            }
        }
        while (childrenConnections.Any())
        {
            IEnumerator<Node> enumerator = childrenConnections.Dequeue();
            while (enumerator.MoveNext())
            {
                yield return enumerator.Current;
            }
        }
        parentConnection.Dispose();
    }

    internal DirectoryLevelOrderTree(DirectoryModel model)
    {
        Model = model;
    }

    protected override DirectoryModel Model { get; }
}
