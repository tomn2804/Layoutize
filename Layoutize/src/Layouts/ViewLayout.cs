using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Layoutize.Annotations;
using Layoutize.Elements;
using Layoutize.Views;

namespace Layoutize.Layouts;

public abstract class ViewLayout : Layout
{
	public bool DeleteOnUnmount { get; init; }

	[Required]
	[Name]
	public string Name
	{
		get
		{
			Debug.Assert(Contexts.Name.IsValid(_name));
			return _name;
		}
		init => _name = value;
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

	private readonly string? _name;
}
