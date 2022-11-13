using System;
using Layoutize.Contexts;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Layoutize.Elements;
using Layoutize.Views;

namespace Layoutize.Layouts;

[Path]
public abstract class FileSystemLayout : Layout
{
	public bool DeleteOnUnmount { get; init; }

	public EventHandler? OnCreated { get; init; }

	public EventHandler? OnCreating { get; init; }

	public EventHandler? OnDeleted { get; init; }

	public EventHandler? OnDeleting { get; init; }

	public EventHandler? OnMounted { get; init; }

	public EventHandler? OnUnmounting { get; init; }

	internal abstract override FileSystemElement CreateElement();

	internal abstract IView CreateView(IBuildContext context);

	[Required]
	[Name]
	public virtual string Name
	{
		get
		{
			Debug.Assert(Validator.TryValidateProperty(_name, new(this) { MemberName = nameof(Name) }, null));
			return _name!;
		}
		init
		{
			Validator.ValidateProperty(value, new(this) { MemberName = nameof(Name) });
			_name = value;
			Debug.Assert(Name == value);
		}
	}

	private string? _name;
}
