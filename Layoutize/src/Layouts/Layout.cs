using System.ComponentModel.DataAnnotations;
using Layoutize.Elements;

namespace Layoutize.Layouts;

public abstract class Layout
{
	public Layout Inherit
	{
		init
		{
			Validator.ValidateObject(value, new(value));
			foreach (var property in value.GetType().GetProperties())
			{
				GetType().GetProperty(property.Name)?.SetValue(this, property.GetValue(value));
			}
		}
	}

	internal abstract Element CreateElement();
}
