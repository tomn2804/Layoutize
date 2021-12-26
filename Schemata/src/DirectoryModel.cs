using System;
using System.Collections.Generic;

namespace Schemata;

public class DirectoryModel : Model
{
    protected DirectoryModel(Blueprint blueprint)
        : base(blueprint)
    {
        Connector.Builder builder = new();
        builder.Processing.Push((object? sender, Connector.ProcessingEventArgs args) => Console.WriteLine("Processing Directory " + Name));
        builder.Processed.Enqueue((object? sender, Connector.ProcessedEventArgs args) => Console.WriteLine("Processed Directory " + Name));

        Connections = Connections.SetItem(DefaultConnector.Mount, builder.ToConnector());

        Network = new(this);
    }

    public List<Model> Children { get; } = new();

    public override DirectoryNetwork Network { get; }
}
