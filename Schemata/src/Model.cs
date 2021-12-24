using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;

namespace Schemata;

public abstract partial class Model : Blueprint.Owner
{
    protected Model(Blueprint blueprint)
        : base(blueprint)
    {
        Debug.Assert(Blueprint.ModelType == GetType());
    }

    public ImmutableDictionary<object, Connection> Connections { get; protected set; } = ImmutableDictionary.Create<object, Connection>();

    public abstract Network Network { get; }
}

public class FileModel : Model
{
    public FileModel(Blueprint blueprint)
        : base(blueprint)
    {
        Connections = Connections.SetItem("Mount", new Connection());
        Network = new(this);
    }

    public override FileNetwork Network { get; }
}

public class DirectoryModel : Model
{
    public DirectoryModel(Blueprint blueprint)
        : base(blueprint)
    {
        Connections = Connections.SetItem("Mount", new Connection());
        Network = new(this);
    }

    public List<Model> Children { get; set; } = new();

    public override DirectoryNetwork Network { get; }
}
