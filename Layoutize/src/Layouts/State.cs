using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Layoutize.Elements;

namespace Layoutize.Layouts;

public abstract class State
{
	protected internal abstract Layout Build(IBuildContext context);

	protected void SetState(Action action)
	{
		Debug.Assert(Model.IsValid(this));
		OnStateUpdating(EventArgs.Empty);
		action.Invoke();
		OnStateUpdated(EventArgs.Empty);
		Debug.Assert(Model.IsValid(this));
	}

	[Required]
	internal StatefulElement Element
	{
		get
		{
			Debug.Assert(_element != null);
			return _element;
		}
		set
		{
			Debug.Assert(!value.IsMounted);
			Debug.Assert(_element == null);
			_element = value;
			Debug.Assert(_element != null);
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
			var layout = (T)Element.Layout;
			Debug.Assert(Model.IsValid(layout));
			return layout;
		}
	}
}
