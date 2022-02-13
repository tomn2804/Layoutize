using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

namespace Layoutize.Layouts;

public abstract class Layout
{
    public ImmutableDictionary<object, object> Attributes { get; }

    protected Layout(IDictionary attributes)
    {
        switch (attributes)
        {
            case ImmutableDictionary<object, object> dictionary:
                Attributes = dictionary;
                break;

            case IDictionary<object, object> dictionary:
                Attributes = dictionary.ToImmutableDictionary();
                break;

            default:
                Attributes = attributes.Cast<DictionaryEntry>().ToImmutableDictionary(entry => entry.Key, entry => entry.Value)!;
                break;
        }
    }

    public abstract Element CreateElement();

    public virtual Element Mount()
    {
        Element element = CreateElement();
        // TODO: Mount
        return element;
    }
}

public abstract class ImmutableLayout : Layout
{
    protected ImmutableLayout(IDictionary attributes)
        : base(attributes)
    {
    }

    protected abstract Layout Build();

    public override Element CreateElement()
    {
        return Build().CreateElement();
    }
}

public abstract class MutableLayout : Layout
{
    protected MutableLayout(IDictionary attributes)
        : base(attributes)
    {
    }

    protected abstract State Build();

    public override Element CreateElement()
    {
        State state = Build();
        Element element = state.CreateElement();
        state.Updated += (object? sender, State.UpdatedEventArgs e) =>
        {
            element.Layout = e.NewLayout;
        };
        return element;
    }
}

public abstract partial class State
{
    public ImmutableDictionary<object, object> Attributes { get; }

    public event EventHandler<UpdatedEventArgs>? Updated;

    protected void SetState(IDictionary values)
    {
        Type type = GetType();
        foreach (DictionaryEntry value in values)
        {
            type.GetProperty(value.Key.ToString()!)!.SetValue(this, value.Value);
        }
        OnUpdated(new(Build()));
    }

    protected virtual void OnUpdated(UpdatedEventArgs args)
    {
        Updated?.Invoke(this, args);
    }

    public State(IDictionary attributes)
    {
        switch (attributes)
        {
            case ImmutableDictionary<object, object> dictionary:
                Attributes = dictionary;
                break;

            case IDictionary<object, object> dictionary:
                Attributes = dictionary.ToImmutableDictionary();
                break;

            default:
                Attributes = attributes.Cast<DictionaryEntry>().ToImmutableDictionary(entry => entry.Key, entry => entry.Value)!;
                break;
        }
    }

    protected abstract Layout Build();

    public virtual Element CreateElement()
    {
        return Build().CreateElement();
    }
}

[AttributeUsage(AttributeTargets.Class)]
public abstract class Schema : Attribute
{
    public string Name { get; } = null!;
}

public class FileSystemSchema : Schema
{
    public int Priority { get; }
    public string Path { get; } = null!;
    public EventHandler OnCreating = null!;
}

public class DirectorySchema : FileSystemSchema
{
    public IEnumerable<Layout> Children { get; } = Array.Empty<Layout>();
}

[FileSystemSchema]
public abstract class FileSystemLayout : ImmutableLayout
{
    public FileSystemLayout(IDictionary attributes)
        : base(attributes)
    {
    }

    protected override Layout Build()
    {
        return this;
    }

    public override FileSystemElement CreateElement()
    {
        throw new NotImplementedException();
    }
}

[FileSystemSchema]
public class FileLayout : FileSystemLayout
{
    public FileLayout(IDictionary attributes)
        : base(attributes)
    {
    }

    public override FileElement CreateElement()
    {
        return new(Attributes.SetItems(new[] { GetOnCreating() }));
    }

    private KeyValuePair<object, object> GetOnCreating()
    {
        EventHandler handler = (object? sender, EventArgs e) =>
        {
            if (Attributes.TryGetValue(nameof(FileSystemSchema.OnCreating), out object? onCreating))
            {
                ((EventHandler)onCreating).Invoke(sender, e);
            }
            // TODO: ...
        };
        return KeyValuePair.Create<object, object>(nameof(FileSystemSchema.OnCreating), handler);
    }
}

public class DirectoryLayout : FileSystemLayout
{
    public DirectoryLayout(IDictionary attributes)
        : base(attributes)
    {
    }

    public override DirectoryElement CreateElement()
    {
        return new(Attributes.SetItems(new[] { GetOnCreating() }));
    }

    private KeyValuePair<object, object> GetOnCreating()
    {
        EventHandler handler = (object? sender, EventArgs e) =>
        {
            if (Attributes.TryGetValue(nameof(FileSystemSchema.OnCreating), out object? onCreating))
            {
                ((EventHandler)onCreating).Invoke(sender, e);
            }
            // TODO: ...
        };
        return KeyValuePair.Create<object, object>(nameof(FileSystemSchema.OnCreating), handler);
    }
}

public partial class Activity
{
    protected Element Element { get; }

    public event EventHandler? Invoking;
    public event EventHandler? Invoked;

    public Activity(Element element)
    {
        Element = element;
    }

    public Action Invoke()
    {
        Invoking?.Invoke(this, EventArgs.Empty);
        return () => Invoked?.Invoke(this, EventArgs.Empty);
    }
}

public class Element
{
    public ImmutableDictionary<object, Activity> Activities { get; init; } = ImmutableDictionary.Create<object, Activity>();

    public Layout Layout { get; set; }

    public Element(IDictionary attributes)
    {
    }
}

public class FileSystemElement : Element
{
    public FileSystemElement(IDictionary attributes)
        : base(attributes)
    {
    }
}

public class FileElement : FileSystemElement
{
    public FileElement(IDictionary attributes)
        : base(attributes)
    {
    }
}

public class DirectoryElement : FileSystemElement
{
    public DirectoryElement(IDictionary attributes)
        : base(attributes)
    {
    }
}
