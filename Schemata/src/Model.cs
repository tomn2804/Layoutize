using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;

namespace Schemata;

public abstract partial class Model : Blueprint.Owner
{
    public ImmutableDictionary<object, Schemata.Activity> Activities { get; protected set; } = ImmutableDictionary.Create<object, Schemata.Activity>();

    public string Name { get; }

    public abstract IEnumerable<Node> Tree { get; }

    protected Model(string path, Blueprint blueprint)
        : base(blueprint)
    {
        Debug.Assert(Blueprint.ModelType == GetType());
        Name = (string)Blueprint.Details["Name"]!;
        Path = path;
    }

    public string Path { get; }
}
