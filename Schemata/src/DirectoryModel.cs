using System;
using System.Collections.Generic;

namespace Schemata;

public class DirectoryModel : Model
{
    public DirectoryModel(Blueprint blueprint)
        : base(blueprint)
    {
        Connections = Connections.SetItem(DefaultConnector.Mount, new Connector());

        Connections[DefaultConnector.Mount].Processing.Push((object? sender, Connector.ProcessingEventArgs args) => Console.WriteLine("Processing Directory " + Name));
        Connections[DefaultConnector.Mount].Processed.Enqueue((object? sender, Connector.ProcessedEventArgs args) => Console.WriteLine("Processed Directory " + Name));

        Network = new(this);
    }

    public List<Model> Children { get; } = new();

    public override DirectoryNetwork Network { get; }
}
