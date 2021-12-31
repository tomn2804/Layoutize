using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;

namespace Schemata;

public abstract partial class Model : Blueprint.Owner
{
    public IReadOnlyDictionary<object, Activity> Activities { get; }

    public string Name { get; }

    public string Path { get; }

    public string FullName => System.IO.Path.Combine(Path, Name);

    public abstract IEnumerable<Node> Tree { get; }

    protected Model(Blueprint blueprint)
        : base(blueprint)
    {
        Debug.Assert(Blueprint.ModelType == GetType());
        Activities = Blueprint.Activities;
        Name = Blueprint.Name;
        Path = Blueprint.Path;
    }
}
