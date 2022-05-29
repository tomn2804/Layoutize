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

	internal virtual bool IsValid()
	{
		return Validator.TryValidateObject(this, new(this), null);
	}

	internal virtual void Validate()
	{
		Validator.ValidateObject(this, new(this));
	}
}
