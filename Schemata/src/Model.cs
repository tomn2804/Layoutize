using System.Collections.Immutable;

namespace Schemata;

public abstract partial class Model : Blueprint.Owner
{
    public ImmutableDictionary<object, Connector> Connections { get; protected set; } = ImmutableDictionary.Create<object, Connector>();

    public string Name { get; }

    public abstract Network Network { get; }

    protected Model(Blueprint blueprint)
        : base(blueprint)
    {
        //Debug.Assert(Blueprint.ModelType == GetType());
        Name = (string)Blueprint.Details["Name"]!;
    }
}
