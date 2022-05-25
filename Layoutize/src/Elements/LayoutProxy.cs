using System;
using System.ComponentModel.DataAnnotations;
using Layoutize.Layouts;

namespace Layoutize.Elements;

internal sealed class LayoutProxy
{
	public Layout Value
	{
		get => _value;
		set
		{
			Validator.ValidateObject(value, new(value));
			OnValueUpdating(EventArgs.Empty);
			_value = value;
			OnValueUpdated(EventArgs.Empty);
		}
	}

	public LayoutProxy(Layout value)
	{
		Validator.ValidateObject(value, new(value));
		_value = value;
	}

	public event EventHandler? ValueUpdated;

	public event EventHandler? ValueUpdating;

	private void OnValueUpdated(EventArgs e)
	{
		ValueUpdated?.Invoke(this, e);
	}

	private void OnValueUpdating(EventArgs e)
	{
		ValueUpdating?.Invoke(this, e);
	}

	private Layout _value;
}
