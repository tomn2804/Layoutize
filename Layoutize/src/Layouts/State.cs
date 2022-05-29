using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Layoutize.Elements;

namespace Layoutize.Layouts;

public abstract class State
{
	[MemberNotNull(nameof(Element))]
	protected internal abstract Layout Build(IBuildContext context);

	[MemberNotNull(nameof(Element))]
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
	internal StatefulElement? Element { get; set; }

	internal event EventHandler? StateUpdated;

	internal event EventHandler? StateUpdating;

	[MemberNotNull(nameof(Element))]
	private protected virtual void OnStateUpdated(EventArgs e)
	{
		Debug.Assert(IsValid());
		StateUpdated?.Invoke(this, e);
		Debug.Assert(IsValid());
	}

	[MemberNotNull(nameof(Element))]
	private protected virtual void OnStateUpdating(EventArgs e)
	{
		Debug.Assert(IsValid());
		StateUpdating?.Invoke(this, e);
		Debug.Assert(IsValid());
	}

	[MemberNotNull(nameof(Element))]
	internal virtual void Validate()
	{
		Validator.ValidateObject(this, new(this));
		Debug.Assert(Element != null);
	}

	[MemberNotNullWhen(true, nameof(Element))]
	internal virtual bool IsValid()
	{
		var result = Validator.TryValidateObject(this, new(this), null);
		if (result) Debug.Assert(Element != null);
		return result;
	}
}

public abstract class State<T> : State where T : StatefulLayout
{
	protected T Layout
	{
		get
		{
			Debug.Assert(IsValid());
			return (T)Element.Layout;
		}
	}
}
