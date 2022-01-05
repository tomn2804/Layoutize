﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Management.Automation;

namespace Schemata;

public abstract partial class Model : Blueprint.Owner
{
    public virtual IReadOnlyDictionary<object, Activity> Activities { get; protected set; } = ImmutableDictionary.Create<object, Activity>();

    public abstract bool Exists { get; }

    public virtual string FullName => System.IO.Path.Combine(Path, Name);

    public bool IsMounted { get; private set; }

    public virtual string Name { get; protected set; }

    public DirectoryModel? Parent { get; private set; }

    public virtual string Path { get; protected set; }

    public virtual int Priority { get; protected set; } = 0;

    public abstract IEnumerable<Node> Tree { get; }

    protected Model(Blueprint blueprint)
        : base(blueprint)
    {
        Path = blueprint.Path;

        Name = (string)blueprint.Details[Template.DetailOption.Name];
        if (string.IsNullOrWhiteSpace(Name))
        {
            throw new ArgumentNullException(nameof(blueprint), $"Details value property '{Template.DetailOption.Name}' cannot be null or containing only white spaces.");
        }

        if (blueprint.Details.TryGetValue(Template.DetailOption.Priority, out object? priorityValue))
        {
            Priority = (int)priorityValue;
        }

        ImmutableDictionary<object, Activity>.Builder builder = ImmutableDictionary.CreateBuilder<object, Activity>();

        builder[ActivityOption.Create] = CreateActivity(Template.DetailOption.OnCreating, Template.DetailOption.OnCreated);
        builder[ActivityOption.Mount] = CreateActivity(Template.DetailOption.OnMounting, Template.DetailOption.OnMounted);

        Activities = builder.ToImmutable();
    }
    private Activity CreateActivity(object processingOption, object processedOption)
    {
        Activity.Builder builder = new();
        if (Blueprint.Details.TryGetValue(processingOption, out object? processingValue))
        {
            EventHandler<Activity.ProcessingEventArgs> handler;
            switch (processingValue)
            {
                case ScriptBlock scriptBlock:
                    handler = (sender, args) => scriptBlock.Invoke(sender, args);
                    break;

                case Action<object?, Activity.ProcessingEventArgs> action:
                    handler = new(action);
                    break;

                default:
                    handler = (EventHandler<Activity.ProcessingEventArgs>)processingValue;
                    break;
            }
            builder.Processing.Push(handler);
        }
        if (Blueprint.Details.TryGetValue(processedOption, out object? processedValue))
        {
            EventHandler<Activity.ProcessedEventArgs> handler;
            switch (processedValue)
            {
                case ScriptBlock scriptBlock:
                    handler = (sender, args) => scriptBlock.Invoke(sender, args);
                    break;

                case Action<object?, Activity.ProcessedEventArgs> action:
                    handler = new(action);
                    break;

                default:
                    handler = (EventHandler<Activity.ProcessedEventArgs>)processedValue;
                    break;
            }
            builder.Processed.Enqueue(handler);
        }
        return builder.ToActivity();
    }
}

public abstract partial class Model : IComparable<Model>
{
    public int CompareTo(Model? other)
    {
        if (other is not null)
        {
            if (Priority != other.Priority)
            {
                return other.Priority.CompareTo(Priority);
            }
            return Name.CompareTo(other.Name);
        }
        return 1;
    }
}
