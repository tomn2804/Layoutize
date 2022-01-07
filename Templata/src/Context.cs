using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Management.Automation;

namespace Templata;

public class Context
{
    public Context()
    {
    }

    public Context(Context other)
    {
        Name = other.Name;
    }

    public string Name { get; init; } = null!;
}

public class FileSystemContext : Context
{
    public FileSystemContext(Context other)
        : base(other)
    {
    }

    public EventHandler OnCreated { get; init; } = null!;
    public EventHandler OnCreating { get; init; } = null!;

    public EventHandler OnDeleted { get; init; } = null!;
    public EventHandler OnDeleting { get; init; } = null!;

    public EventHandler OnMounted { get; init; } = null!;
    public EventHandler OnMounting { get; init; } = null!;

    public string Path { get; init; } = null!;
    public int Priority { get; init; } = 0;
    public Type ViewType { get; init; } = null!;
}

public class FileContext : FileSystemContext
{
    public FileContext(FileSystemContext other)
        : base(other)
    {
    }
}

public class DirectoryContext : FileSystemContext
{
    public DirectoryContext(FileSystemContext other)
        : base(other)
    {
    }

    public IEnumerable<Template> Children { get; init; } = null!;
}

public abstract class Template
{
    public ImmutableDictionary<object, object> Details { get; }

    protected Template(IDictionary details)
    {
        Details = details;
    }

    public static implicit operator Context(Template template)
    {
        return template.GetContext();
    }

    protected virtual Context GetContext()
    {
        return new() { Name = (string)Details[nameof(Context.Name)] };
    }
}

public class FileSystemTemplate : Template
{
    public FileSystemTemplate(IDictionary details)
        : base(details)
    {
    }

    protected override FileSystemContext GetContext()
    {
        int priority = 0;
        if (Details.TryGetValue(nameof(FileSystemContext.Priority), out object? priorityValue))
        {
            priority = (int)priorityValue;
        }

        Type viewType = typeof(FileSystemView);
        if (Details.TryGetValue(nameof(FileSystemContext.ViewType), out object? viewTypeValue))
        {
            viewType = (Type)viewTypeValue;
        }

        return new(base.GetContext())
        {
            OnCreated = GetEventHandler(nameof(FileContext.OnCreated)),
            OnCreating = GetEventHandler(nameof(FileContext.OnCreating)),

            OnDeleted = GetEventHandler(nameof(FileContext.OnDeleted)),
            OnDeleting = GetEventHandler(nameof(FileContext.OnDeleting)),

            OnMounted = GetEventHandler(nameof(FileContext.OnMounted)),
            OnMounting = GetEventHandler(nameof(FileContext.OnMounting)),

            Path = (string)Details[nameof(FileContext.Path)],
            Priority = priority,
            ViewType = viewType
        };
    }

    private EventHandler GetEventHandler(string name)
    {
        return (object? sender, EventArgs args) =>
        {
            if (Details.TryGetValue(name, out object? handlerValue))
            {
                switch (handlerValue)
                {
                    case ScriptBlock scriptBlock:
                        scriptBlock.Invoke(sender, args);
                        break;
                    default:
                        ((EventHandler)handlerValue).Invoke(sender, args);
                        break;
                }
            }
        };
    }
}

public class FileTemplate : Template
{
    public FileTemplate(IDictionary details)
        : base(details)
    {
    }

    protected override FileContext GetContext()
    {
        return new((FileSystemContext)new FileSystemTemplate(Details.SetItems(new[] { GetOnCreatingDetail(), GetOnMountingDetail(), GetViewTypeDetail() })));
    }

    private KeyValuePair<object, object> GetViewTypeDetail()
    {
        return KeyValuePair.Create<object, object>(nameof(FileSystemContext.ViewType), typeof(FileView));
    }

    private KeyValuePair<object, object> GetOnCreatingDetail()
    {
        EventHandler handler = (object? sender, EventArgs args) =>
        {
            if (Details.TryGetValue(nameof(FileSystemContext.OnCreating), out object? onCreatingValue))
            {
                switch (onCreatingValue)
                {
                    case ScriptBlock scriptBlock:
                        scriptBlock.Invoke(sender, args);
                        break;

                    default:
                        ((EventHandler)onCreatingValue).Invoke(sender, args);
                        break;
                }
            }
            Node node = (Node)sender!;
            File.Create(node.View.FullName).Dispose();
        };
        return KeyValuePair.Create<object, object>(nameof(FileSystemContext.OnCreating), handler);
    }

    private KeyValuePair<object, object> GetOnDeletingDetail()
    {
        EventHandler handler = (object? sender, EventArgs args) =>
        {
            if (Details.TryGetValue(nameof(FileSystemContext.OnDeleting), out object? onDeletingValue))
            {
                switch (onDeletingValue)
                {
                    case ScriptBlock scriptBlock:
                        scriptBlock.Invoke(sender, args);
                        break;

                    default:
                        ((EventHandler)onDeletingValue).Invoke(sender, args);
                        break;
                }
            }
            Node node = (Node)sender!;
            File.Delete(node.View.FullName);
        };
        return KeyValuePair.Create<object, object>(nameof(FileSystemContext.OnDeleting), handler);
    }

    private KeyValuePair<object, object> GetOnMountingDetail()
    {
        EventHandler handler = (object? sender, EventArgs args) =>
        {
            if (Details.TryGetValue(nameof(FileSystemContext.OnMounting), out object? onMountingValue))
            {
                switch (onMountingValue)
                {
                    case ScriptBlock scriptBlock:
                        scriptBlock.Invoke(sender, args);
                        break;

                    default:
                        ((EventHandler)onMountingValue).Invoke(sender, args);
                        break;
                }
            }
            Node node = (Node)sender!;
            if (!node.View.Exists)
            {
                node.Invoke(node.View.Activities[View.ActivityOption.Create]);
            }
        };
        return KeyValuePair.Create<object, object>(nameof(FileSystemContext.OnMounting), handler);
    }
}

public class DirectoryTemplate : Template
{
    public DirectoryTemplate(IDictionary details)
        : base(details)
    {
    }

    protected override DirectoryContext GetContext()
    {
        return new((FileSystemContext)new FileSystemTemplate(Details.SetItems(new[] { GetOnCreatingDetail(), GetOnMountingDetail(), GetViewTypeDetail() })));
    }

    private KeyValuePair<object, object> GetViewTypeDetail()
    {
        return KeyValuePair.Create<object, object>(nameof(FileSystemContext.ViewType), typeof(DirectoryView));
    }

    private KeyValuePair<object, object> GetOnCreatingDetail()
    {
        EventHandler handler = (object? sender, EventArgs args) =>
        {
            if (Details.TryGetValue(nameof(FileSystemContext.OnCreating), out object? onCreatingValue))
            {
                switch (onCreatingValue)
                {
                    case ScriptBlock scriptBlock:
                        scriptBlock.Invoke(sender, args);
                        break;

                    default:
                        ((EventHandler)onCreatingValue).Invoke(sender, args);
                        break;
                }
            }
            Node node = (Node)sender!;
            Directory.CreateDirectory(node.View.FullName);
        };
        return KeyValuePair.Create<object, object>(nameof(FileSystemContext.OnCreating), handler);
    }

    private KeyValuePair<object, object> GetOnDeletingDetail()
    {
        EventHandler handler = (object? sender, EventArgs args) =>
        {
            if (Details.TryGetValue(nameof(FileSystemContext.OnDeleting), out object? onDeletingValue))
            {
                switch (onDeletingValue)
                {
                    case ScriptBlock scriptBlock:
                        scriptBlock.Invoke(sender, args);
                        break;

                    default:
                        ((EventHandler)onDeletingValue).Invoke(sender, args);
                        break;
                }
            }
            Node node = (Node)sender!;
            Directory.Delete(node.View.FullName);
        };
        return KeyValuePair.Create<object, object>(nameof(FileSystemContext.OnDeleting), handler);
    }

    private KeyValuePair<object, object> GetOnMountingDetail()
    {
        EventHandler handler = (object? sender, EventArgs args) =>
        {
            if (Details.TryGetValue(nameof(FileSystemContext.OnMounting), out object? onMountingValue))
            {
                switch (onMountingValue)
                {
                    case ScriptBlock scriptBlock:
                        scriptBlock.Invoke(sender, args);
                        break;

                    default:
                        ((EventHandler)onMountingValue).Invoke(sender, args);
                        break;
                }
            }
            Node node = (Node)sender!;
            if (!node.View.Exists)
            {
                node.Invoke(node.View.Activities[View.ActivityOption.Create]);
            }
            Children.AddRange(Context.Children);
        };
        return KeyValuePair.Create<object, object>(nameof(FileSystemContext.OnMounting), handler);
    }
}

public abstract class View
{
    protected virtual Context Context { get; }

    public string Name => Context.Name;

    protected View(Context context)
    {
        Context = context;
    }
}

public abstract class FileSystemView : View
{
    protected override FileSystemContext Context => (FileSystemContext)base.Context;

    public string FullName => Path.Combine(Context.Path, Name);

    public bool Exists => File.Exists(FullName);

    public bool IsMounted { get; private set; }

    protected FileSystemView(FileSystemContext context)
        : base(context)
    {
    }

    public virtual Action Create()
    {
        Context.OnCreating.Invoke(this, EventArgs.Empty);
        return () => Context.OnCreated.Invoke(this, EventArgs.Empty);
    }

    public virtual Action Delete()
    {
        Context.OnDeleting.Invoke(this, EventArgs.Empty);
        return () => Context.OnDeleted.Invoke(this, EventArgs.Empty);
    }

    public virtual Action Mount()
    {
        Context.OnMounting.Invoke(this, EventArgs.Empty);
        IsMounted = true;
        return () => Context.OnMounted.Invoke(this, EventArgs.Empty);
    }
}

public class FileView : FileSystemView
{
    protected override FileContext Context => (FileContext)base.Context;

    public FileView(FileContext context)
        : base(context)
    {
    }
}

public class DirectoryView : FileSystemView
{
    protected override DirectoryContext Context => (DirectoryContext)base.Context;

    public ChildrenSet Children { get; }

    public DirectoryView(DirectoryContext context)
        : base(context)
    {
        Children = new(this);
    }
}
