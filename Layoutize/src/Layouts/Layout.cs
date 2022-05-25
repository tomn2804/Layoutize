using Layoutize.Elements;

namespace Layoutize.Layouts;

public abstract class Layout
{
	public Layout Inherit
	{
		init
		{
			foreach (var property in value.GetType().GetProperties())
			{
				GetType().GetProperty(property.Name)?.SetValue(this, property.GetValue(value));
			}
		}
	}

	internal abstract Element CreateElement();
}
