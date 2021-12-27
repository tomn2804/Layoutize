using System;
using System.Linq;
using System.Collections.Generic;

namespace Schemata;

public class DirectoryModel : Model
{
    public List<Model> Children { get; } = new();

    public override DirectoryLevelOrderNetwork Network { get; }

    public DirectoryModel(Blueprint blueprint)
        : base(blueprint)
    {
        Connector.Builder builder = new();
        builder.Processing.Push((object? sender, Connector.ProcessingEventArgs args) => Console.WriteLine("Processing Directory " + Name));
        builder.Processed.Enqueue((object? sender, Connector.ProcessedEventArgs args) => Console.WriteLine("Processed Directory " + Name));

        Connections = Connections.SetItem(DefaultConnector.Mount, builder.ToConnector());

        Network = new(this);
    }
}
