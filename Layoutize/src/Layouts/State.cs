using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Layoutize.Contexts;
using Layoutize.Elements;
using Layoutize.Utils;

namespace Layoutize.Layouts;

public abstract class State
{
	protected internal abstract Layout Build(IBuildContext context);

	protected void SetState(Action action)
	{
		Debug.Assert(Model.IsValid(this));
		OnStateUpdating(EventArgs.Empty);
		action.Invoke();
		Model.Validate(this);
		OnStateUpdated(EventArgs.Empty);
	}

	[Required]
	internal StatefulElement Element
	{
		get
		{
			Debug.Assert(Validator.TryValidateProperty(_element, new(this) { MemberName = nameof(Element) }, null));
			return _element!;
		}
		set
		{
			Debug.Assert(Validator.TryValidateProperty(value, new(this) { MemberName = nameof(Element) }, null));
			Debug.Assert(!value.IsMounted);
			_element = value;
			Debug.Assert(Element == value);
		}
	}

	internal event EventHandler? StateUpdated;

	internal event EventHandler? StateUpdating;

	private StatefulElement? _element;

	private protected virtual void OnStateUpdated(EventArgs e)
	{
		Debug.Assert(Model.IsValid(this));
		StateUpdated?.Invoke(this, e);
		Debug.Assert(Model.IsValid(this));
	}

	private protected virtual void OnStateUpdating(EventArgs e)
	{
		Debug.Assert(Model.IsValid(this));
		StateUpdating?.Invoke(this, e);
		Debug.Assert(Model.IsValid(this));
	}
}

public abstract class State<T> : State where T : StatefulLayout
{
	protected T Layout
	{
		get
		{
			Debug.Assert(Model.IsValid(this));
			return (T)Element.Layout;
		}
	}
}
