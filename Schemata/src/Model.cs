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

    public ImmutableDictionary<object, Connection> Connections { get; protected set; } = ImmutableDictionary.Create<object, Connection>();

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
        Connections = Connections.SetItem(DefaultConnection.Mount, new Connection());

        Connections[DefaultConnection.Mount].Processing += (object? sender, Connection.ProcessingEventArgs args) => Console.WriteLine("Processing File " + Name);
        Connections[DefaultConnection.Mount].Processed += (object? sender, Connection.ProcessedEventArgs args) => Console.WriteLine("Processed File" + Name);

        Network = new(this);
    }

    public override FileNetwork Network { get; }
}

public class DirectoryModel : Model
{
    public DirectoryModel(Blueprint blueprint)
        : base(blueprint)
    {
        Connections = Connections.SetItem(DefaultConnection.Mount, new Connection());

        Connections[DefaultConnection.Mount].Processing += (object? sender, Connection.ProcessingEventArgs args) => Console.WriteLine("Processing Directory" + Name);
        Connections[DefaultConnection.Mount].Processed += (object? sender, Connection.ProcessedEventArgs args) => Console.WriteLine("Processed Directory" + Name);

        Network = new(this);
    }

    public List<Model> Children { get; set; } = new();

    public override DirectoryNetwork Network { get; }
}
