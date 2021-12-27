using System;
using System.Collections.Generic;
using System.Linq;

namespace Schemata;

public sealed class DirectoryLevelOrderNetwork : Network
{
    public override IEnumerator<Connection> GetEnumerator()
    {
        Connection parentConnection = new(Model);
        yield return parentConnection;
        Queue<IEnumerator<Connection>> childrenConnections = new(Model.Children.Count);
        foreach (Model child in Model.Children)
        {
            IEnumerator<Connection> enumerator = child.Network.GetEnumerator();
            if (enumerator.MoveNext())
            {
                childrenConnections.Enqueue(enumerator);
                yield return enumerator.Current;
            }
        }
        while (childrenConnections.Any())
        {
            IEnumerator<Connection> enumerator = childrenConnections.Dequeue();
            while (enumerator.MoveNext())
            {
                yield return enumerator.Current;
            }
        }
        parentConnection.Dispose();
    }

    internal DirectoryLevelOrderNetwork(DirectoryModel model)
    {
        Model = model;
    }

    protected override DirectoryModel Model { get; }
}
