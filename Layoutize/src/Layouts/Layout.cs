using System.ComponentModel.DataAnnotations;
using Layoutize.Elements;

namespace Layoutize.Layouts;

public abstract class Layout
{
	public Layout Inherit
	{
		init
		{
			value.Validate();
			foreach (var property in value.GetType().GetProperties())
			{
				GetType().GetProperty(property.Name)?.SetValue(this, property.GetValue(value));
			}
		}
	}

	internal abstract Element CreateElement();

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
}
