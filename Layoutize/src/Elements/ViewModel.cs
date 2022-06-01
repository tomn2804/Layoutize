using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Layoutize.Layouts;

namespace Layoutize.Elements;

internal sealed class ViewModel
{
	public ViewModel(Layout layout)
	{
		Validator.ValidateObject(layout, new(layout));
		_layout = layout;
	}

	public Layout Layout
	{
		get
		{
			Debug.Assert(Validator.TryValidateObject(_layout, new(_layout), null));
			return _layout;
		}
		set
		{
			Validator.ValidateObject(value, new(value));
			OnLayoutUpdating(EventArgs.Empty);
			_layout = value;
			OnLayoutUpdated(EventArgs.Empty);
		}
	}

	public event EventHandler? LayoutUpdated;

	public event EventHandler? LayoutUpdating;

	private void OnLayoutUpdated(EventArgs e)
	{
		LayoutUpdated?.Invoke(this, e);
	}

	private void OnLayoutUpdating(EventArgs e)
	{
		LayoutUpdating?.Invoke(this, e);
	}

	private Layout _layout;
}
