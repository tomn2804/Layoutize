using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Schemata;

public abstract partial class Model : Blueprint.Owner
{
    public IReadOnlyDictionary<object, Activity> Activities { get; }

    public string Name { get; }

    public string Path { get; }

    public int Priority { get; } = 0;

    public string FullName => System.IO.Path.Combine(Path, Name);

    public abstract IEnumerable<Node> Tree { get; }

    protected Model(Blueprint blueprint)
        : base(blueprint)
    {
        Debug.Assert(Blueprint.ModelType == GetType());
        Activities = Blueprint.Activities;
        Name = (string)blueprint.Details[Template.RequiredDetails.Name];
        Path = Blueprint.Path;
        if (blueprint.Details.TryGetValue(Template.OptionalDetails.Priority, out object? priorityValue) && priorityValue is int priority)
        {
            Priority = priority;
        }
    }
}

public abstract partial class Model : IComparable<Model>
{
    public int CompareTo(Model? other)
    {
        if (other is not null)
        {
            if ((Priority != 0) || (other.Priority != 0))
            {
                return other.Priority.CompareTo(Priority);
            }
            return Name.CompareTo(other.Name);
        }
        return 1;
    }
}

public abstract partial class Model
{
    public class Comparer : IComparer<Model>
    {
        public int Compare(Model? x, Model? y)
        {
            return x?.CompareTo(y) ?? y?.CompareTo(x) ?? 0;
        }
    }
}
