using System;

namespace Schemata;

public class FileModel : Model
{
    public FileModel(Blueprint blueprint)
        : base(blueprint)
    {
        Connections = Connections.SetItem(DefaultConnector.Mount, new Connector());

        Connections[DefaultConnector.Mount].Processing.Push((object? sender, Connector.ProcessingEventArgs args) => Console.WriteLine("Processing File " + Name));
        Connections[DefaultConnector.Mount].Processed.Enqueue((object? sender, Connector.ProcessedEventArgs args) => Console.WriteLine("Processed File" + Name));

        Network = new(this);
    }

    public override FileNetwork Network { get; }
}
