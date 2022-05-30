using System;
using System.Diagnostics;
using Layoutize.Layouts;

namespace Layoutize.Elements;

internal sealed class ViewModel
{
	public ViewModel(Layout layout)
	{
		Debug.Assert(layout.IsValid());
		_layout = layout;
	}

	public Layout Layout
	{
		get => _layout;
		set
		{
			value.Validate();
			OnValueUpdating(EventArgs.Empty);
			_layout = value;
			OnValueUpdated(EventArgs.Empty);
		}
	}

	public event EventHandler? LayoutUpdated;

	public event EventHandler? LayoutUpdating;

	private void OnValueUpdated(EventArgs e)
	{
		LayoutUpdated?.Invoke(this, e);
	}

	private void OnValueUpdating(EventArgs e)
	{
		LayoutUpdating?.Invoke(this, e);
	}

	private Layout _layout;
}
