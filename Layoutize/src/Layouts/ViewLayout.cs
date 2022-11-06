using System;
using Layoutize.Contexts;
using Layoutize.Elements;
using Layoutize.Views;

namespace Layoutize.Layouts;

public abstract class ViewLayout : Layout
{
	public bool DeleteOnUnmount { get; init; }

	public EventHandler? OnCreated { get; init; }

	public EventHandler? OnCreating { get; init; }

	public EventHandler? OnDeleted { get; init; }

	public EventHandler? OnDeleting { get; init; }

	public EventHandler? OnMounted { get; init; }

	public EventHandler? OnUnmounting { get; init; }

	internal abstract override ViewElement CreateElement();

	internal abstract IView CreateView(IBuildContext context);
}
