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
		OnStateUpdating(EventArgs.Empty);
		action.Invoke();
		Validate();
		OnStateUpdated(EventArgs.Empty);
	}

	internal bool IsValid()
	{
		try
		{
			Validate();
		}
		catch
		{
			return false;
		}
		return true;
	}

	internal void Validate()
	{
		Validator.ValidateObject(this, new(this));
	}

	// TODO: Replace this with ViewModel
	[Required]
	internal StatefulElement Element
	{
		get
		{
			Debug.Assert(_element != null);
			return _element;
		}
		set => _element = value;
	}

	internal event EventHandler? StateUpdated;

	internal event EventHandler? StateUpdating;

	private StatefulElement? _element;

	private protected virtual void OnStateUpdated(EventArgs e)
	{
		Debug.Assert(IsValid());
		StateUpdated?.Invoke(this, e);
		Debug.Assert(IsValid());
	}

	private protected virtual void OnStateUpdating(EventArgs e)
	{
		Debug.Assert(IsValid());
		StateUpdating?.Invoke(this, e);
		Debug.Assert(IsValid());
	}
}

public abstract class State<T> : State where T : StatefulLayout
{
	protected T Layout => (T)Element.Layout;
}
