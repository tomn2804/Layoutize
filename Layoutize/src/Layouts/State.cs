using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Layoutize.Elements;

namespace Layoutize.Layouts;

public abstract class State
{
	protected internal abstract Layout Build(IBuildContext context);

	protected void SetState(Action action)
	{
		Debug.Assert(IsValid());
		OnStateUpdating(EventArgs.Empty);
		action.Invoke();
		Validate();
		OnStateUpdated(EventArgs.Empty);
		Debug.Assert(IsValid());
	}

	[Required]
	internal StatefulLayout? Layout { get; set; }

	internal event EventHandler? StateUpdated;

	internal event EventHandler? StateUpdating;

	private protected virtual void OnStateUpdated(EventArgs e)
	{
		Debug.Assert(IsValid());
		StateUpdated?.Invoke(this, e);
	}

	private protected virtual void OnStateUpdating(EventArgs e)
	{
		Debug.Assert(IsValid());
		StateUpdating?.Invoke(this, e);
	}

	[MemberNotNull(nameof(Layout))]
	internal virtual void Validate()
	{
		Validator.ValidateObject(this, new(this));
		Debug.Assert(Layout != null);
		Debug.Assert(Layout.IsValid());
	}

	[MemberNotNullWhen(true, nameof(Layout))]
	internal virtual bool IsValid()
	{
		var result = Validator.TryValidateObject(this, new(this), null);
		Debug.Assert(Layout != null);
		Debug.Assert(Layout.IsValid());
		return result;
	}
}

public abstract class State<T> : State where T : StatefulLayout
{
	protected new T? Layout => (T?)base.Layout;
}
