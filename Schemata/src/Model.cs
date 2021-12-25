using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;

namespace Schemata;

public abstract partial class Model : Blueprint.Owner
{
    public string Name { get; }

    protected Model(Blueprint blueprint)
        : base(blueprint)
    {
        //Debug.Assert(Blueprint.ModelType == GetType());
        Name = (string)Blueprint.Details["Name"]!;
    }

    public ImmutableDictionary<object, Connector> Connections { get; protected set; } = ImmutableDictionary.Create<object, Connector>();

    public abstract Network Network { get; }
}

public abstract partial class Model
{
    public enum DefaultConnection
    {
        Mount
    }
}

public class FileModel : Model
{
    public FileModel(Blueprint blueprint)
        : base(blueprint)
    {
        Connections = Connections.SetItem(DefaultConnection.Mount, new Connector());

        Connections[DefaultConnection.Mount].Processing.Push((object? sender, Connector.ProcessingEventArgs args) => Console.WriteLine("Processing File " + Name));
        Connections[DefaultConnection.Mount].Processed.Enqueue((object? sender, Connector.ProcessedEventArgs args) => Console.WriteLine("Processed File" + Name));

        Network = new(this);
    }

    public override FileNetwork Network { get; }
}

public class DirectoryModel : Model
{
    public DirectoryModel(Blueprint blueprint)
        : base(blueprint)
    {
        Connections = Connections.SetItem(DefaultConnection.Mount, new Connector());

        Connections[DefaultConnection.Mount].Processing.Push((object? sender, Connector.ProcessingEventArgs args) => Console.WriteLine("Processing Directory " + Name));
        Connections[DefaultConnection.Mount].Processed.Enqueue((object? sender, Connector.ProcessedEventArgs args) => Console.WriteLine("Processed Directory " + Name));

        Network = new(this);
    }

    public List<Model> Children { get; set; } = new();

    public override DirectoryNetwork Network { get; }
}
