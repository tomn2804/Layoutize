using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Management.Automation;

namespace Templatize;

public abstract partial class View
{
    public class ActivityOption
    {
        public const string Create = nameof(Create);
        public const string Mount = nameof(Mount);
    }
}

public abstract partial class View : Layout.Owner
{
    public virtual IReadOnlyDictionary<object, Activity> Activities { get; } = new();

    public abstract bool Exists { get; }

    public virtual string FullName => System.IO.Path.Combine(Path, Name);

    public bool IsMounted { get; private set; }

    public virtual string Name { get; protected set; }

    public DirectoryView? Parent { get; private set; }

    public virtual string Path { get; protected set; }

    public virtual int Priority { get; protected set; } = 0;

    public abstract IEnumerable<Node> Tree { get; }

    protected View(Layout layout)
        : base(layout)
    {
        Activities = layout.Activities;
        Name = layout.Name;
        Path = layout.Path;
        Priority = layout.Priority;
    }

    private Activity CreateActivity(object processingOption, object processedOption)
    {
        Activity.Builder builder = new();
        if (Layout.Details.TryGetValue(processingOption, out object? processingValue))
        {
            EventHandler<Activity.InvokingEventArgs> handler;
            switch (processingValue)
            {
                case ScriptBlock scriptBlock:
                    handler = (sender, args) => scriptBlock.Invoke(sender, args);
                    break;

                case Action<object?, Activity.InvokingEventArgs> action:
                    handler = new(action);
                    break;

                default:
                    handler = (EventHandler<Activity.InvokingEventArgs>)processingValue;
                    break;
            }
            builder.Invoking.Push(handler);
        }
        if (Layout.Details.TryGetValue(processedOption, out object? processedValue))
        {
            EventHandler<Activity.InvokedEventArgs> handler;
            switch (processedValue)
            {
                case ScriptBlock scriptBlock:
                    handler = (sender, args) => scriptBlock.Invoke(sender, args);
                    break;

                case Action<object?, Activity.InvokedEventArgs> action:
                    handler = new(action);
                    break;

                default:
                    handler = (EventHandler<Activity.InvokedEventArgs>)processedValue;
                    break;
            }
            builder.Invoked.Enqueue(handler);
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
