using System;
using System.Collections.Generic;

namespace Schemata;

public class FileModel : Model
{
    public override FileNetwork Network { get; }

    protected FileModel(Blueprint blueprint)
        : base(blueprint)
    {
        Connector.Builder builder = new();
        builder.Processing.Push((object? sender, Connector.ProcessingEventArgs args) => Console.WriteLine("Processing File " + Name));
        builder.Processed.Enqueue((object? sender, Connector.ProcessedEventArgs args) => Console.WriteLine("Processed File " + Name));

        Connections = Connections.SetItem(DefaultConnector.Mount, builder.ToConnector());

        Network = new(this);
    }
}
