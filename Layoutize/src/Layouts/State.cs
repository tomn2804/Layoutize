using System;
using System.ComponentModel.DataAnnotations;
using Layoutize.Elements;

namespace Layoutize.Layouts;

public abstract class State
{
	protected internal abstract Layout Build(IBuildContext context);

	protected void SetState(Action action)
	{
		OnStateUpdating(EventArgs.Empty);
		action.Invoke();
		OnStateUpdated(EventArgs.Empty);
	}

	[Required]
	internal StatefulElement Element { get; set; } = null!;

	internal event EventHandler? StateUpdated;

	internal event EventHandler? StateUpdating;

	private protected virtual void OnStateUpdated(EventArgs e)
	{
		StateUpdated?.Invoke(this, e);
	}

	private protected virtual void OnStateUpdating(EventArgs e)
	{
		StateUpdating?.Invoke(this, e);
	}
}

public abstract class State<T> : State where T : StatefulLayout
{
	protected T Layout => (T)Element.Layout;
}
