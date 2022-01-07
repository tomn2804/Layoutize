using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Management.Automation;

namespace Templata;

public abstract partial class View
{
    public class ActivityOption
    {
        public const string Create = nameof(Create);
        public const string Mount = nameof(Mount);
    }
}

public abstract partial class View : Context.Owner
{
    public virtual IReadOnlyDictionary<object, Activity> Activities { get; protected set; } = ImmutableDictionary.Create<object, Activity>();

    public abstract bool Exists { get; }

    public virtual string FullName => System.IO.Path.Combine(Path, Name);

    public bool IsMounted { get; private set; }

    public virtual string Name { get; protected set; }

    public DirectoryView? Parent { get; private set; }

    public virtual string Path { get; protected set; }

    public virtual int Priority { get; protected set; } = 0;

    public abstract IEnumerable<Node> Tree { get; }

    protected View(Context context)
        : base(context)
    {
        Path = context.Path;

        Name = (string)context.Details[Template.DetailOption.Name];
        if (string.IsNullOrWhiteSpace(Name))
        {
            throw new ArgumentNullException(nameof(context), $"Details value property '{Template.DetailOption.Name}' cannot be null or containing only white spaces.");
        }

        if (context.Details.TryGetValue(Template.DetailOption.Priority, out object? priorityValue))
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
        if (Context.Details.TryGetValue(processingOption, out object? processingValue))
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
        if (Context.Details.TryGetValue(processedOption, out object? processedValue))
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

public abstract partial class View : IComparable<View>
{
    public int CompareTo(View? other)
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
