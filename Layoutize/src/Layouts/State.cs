using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Layoutize.Annotations;
using Layoutize.Elements;

namespace Layoutize.Layouts;

public abstract class State
{
	protected internal abstract Layout Build(IBuildContext context);

	protected void SetState(Action action)
	{
		Debug.Assert(this.IsValid());
		OnStateUpdating(EventArgs.Empty);
		action.Invoke();
		this.Validate();
		OnStateUpdated(EventArgs.Empty);
	}

	[Required]
	internal StatefulElement Element
	{
		get
		{
			Debug.Assert(this.IsMemberValid(nameof(Element), _element));
			return _element;
		}
		set
		{
			Debug.Assert(this.IsMemberValid(nameof(Element), value));
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
		Debug.Assert(this.IsValid());
		StateUpdated?.Invoke(this, e);
	}

	private protected virtual void OnStateUpdating(EventArgs e)
	{
		Debug.Assert(this.IsValid());
		StateUpdating?.Invoke(this, e);
	}
}

public abstract class State<T> : State where T : StatefulLayout
{
	protected T Layout
	{
		get
		{
			Debug.Assert(this.IsValid());
			return (T)Element.Layout;
		}
	}
}
