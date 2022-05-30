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

	[Required]
	internal ViewModel ViewModel
	{
		get
		{
			Debug.Assert(_viewModel != null);
			return _viewModel;
		}
		set
		{
			if (_viewModel != null) throw new InvalidOperationException("Viewmodel is already initialized.");
			_viewModel = value;
		}
	}

	internal event EventHandler? StateUpdated;

	internal event EventHandler? StateUpdating;

	private ViewModel? _viewModel;

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
	protected T Layout
	{
		get
		{
			Debug.Assert(IsValid()); // TODO: Move to Element getter
			return (T)ViewModel.Layout;
		}
	}
}
