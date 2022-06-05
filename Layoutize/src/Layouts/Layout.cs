using System.Linq;
using System.Reflection;
using Layoutize.Elements;

namespace Layoutize.Layouts;

public abstract class Layout
{
	public Layout Inherit
	{
		init
		{
			Model.Validate(value);
			const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public;
			var thisProperties = GetType().GetProperties(flags);
			foreach (var otherProperty in value.GetType().GetProperties(flags))
			{
				var thisProperty
					= thisProperties.LastOrDefault(thisProperty => thisProperty.Name == otherProperty.Name);
				if (thisProperty != null && thisProperty.CanWrite && otherProperty.CanRead)
				{
					thisProperty.SetValue(this, otherProperty.GetValue(value));
				}
			}
		}
	}

	internal abstract Element CreateElement(Element? parent);
}
