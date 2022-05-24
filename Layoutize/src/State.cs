using System;
using System.Collections;
using System.Diagnostics;
using Layoutize.Elements;

namespace Layoutize;

public abstract class State
{
	protected internal abstract Layout Build(IBuildContext context);

	protected void SetState(IDictionary properties)
	{
		OnStateUpdating(EventArgs.Empty);
		foreach (DictionaryEntry property in properties)
		{
			GetType().GetProperty((string)property.Key)!.SetValue(this, property.Value);
		}
		OnStateUpdated(EventArgs.Empty);
	}

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
