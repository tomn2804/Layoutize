using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Schemata;

public abstract partial class Model : Blueprint.Owner
{
    public IReadOnlyDictionary<object, Activity> Activities { get; } = ImmutableDictionary.Create<object, Activity>();

    public string FullName => System.IO.Path.Combine(Path, Name);

    public string Name { get; }

    public DirectoryModel? Parent { get; private set; }

    public string Path { get; }

    public int Priority { get; } = 0;

    public abstract IEnumerable<Node> Tree { get; }

    protected Model(Blueprint blueprint)
        : base(blueprint)
    {
        Name = (string)blueprint.Details[Template.DetailOption.Name];
        if (string.IsNullOrWhiteSpace(Name))
        {
            throw new ArgumentNullException(nameof(blueprint), $"Details value property '{Template.DetailOption.Name}' cannot be null or containing only white spaces.");
        }

        Path = (string)blueprint.Details[Template.DetailOption.Path];
        if (string.IsNullOrWhiteSpace(Path))
        {
            throw new ArgumentNullException(nameof(blueprint), $"Details value property '{Template.DetailOption.Path}' cannot be null or containing only white spaces.");
        }
        if (Path.IndexOfAny(System.IO.Path.GetInvalidPathChars()) != -1)
        {
            throw new ArgumentException($"Details value property '{Template.DetailOption.Path}' cannot contain invalid system characters.", nameof(blueprint));
        }

        if (blueprint.Details.TryGetValue(Template.DetailOption.Activities, out object? activitiesValue))
        {
            Activities = (IReadOnlyDictionary<object, Activity>)activitiesValue;
        }
        if (blueprint.Details.TryGetValue(Template.DetailOption.Priority, out object? priorityValue))
        {
            Priority = (int)priorityValue;
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
