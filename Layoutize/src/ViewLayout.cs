using Layoutize.Elements;
using Layoutize.Views;
using System;

namespace Layoutize;

public abstract class ViewLayout : Layout
{
    private string? _name;

    public bool DeleteOnUnmount { get; init; }

    public string Name
    {
        get => _name ?? throw new InvalidOperationException("Attribute 'Name' is not initialized.");
        init
        {
            Utils.Name.Validate(value);
            _name = value;
        }
    }

    public EventHandler? OnCreated { get; init; }

    public EventHandler? OnCreating { get; init; }

    public EventHandler? OnDeleted { get; init; }

    public EventHandler? OnDeleting { get; init; }

    public EventHandler? OnMounted { get; init; }

    public EventHandler? OnMounting { get; init; }

    public EventHandler? OnUnmounted { get; init; }

    public EventHandler? OnUnmounting { get; init; }

    internal abstract override ViewElement CreateElement();

    internal abstract View CreateView(IBuildContext context);
}
