using Layoutize.Elements;
using System;
using System.Linq;

namespace Layoutize.Annotations;

public static class Selector<T>
{
	public static T? GetValue(IBuildContext context, Type attributeType, bool inclusive = false)
	{
		T? value = default;
		void visitElement(Element? element)
		{
			if (element != null)
			{
				var layoutType = element.Layout.GetType();
				if (Attribute.GetCustomAttribute(layoutType, attributeType) is IContextValue<T> classAttribute)
				{
					if (!classAttribute.TryGetValue(element, out value))
					{
						visitElement(element.Parent);
					}
					return;
				}
				var propertyType = layoutType.GetProperties().FirstOrDefault(property => Attribute.IsDefined(property, attributeType));
				if (propertyType != null && Attribute.GetCustomAttribute(propertyType, attributeType) is IContextValue<T> propertyAttribute)
				{
					if (!propertyAttribute.TryGetValue(element, out value))
					{
						visitElement(element.Parent);
					}
					return;
				}
			}
		}
		var element = context.Element;
		visitElement(inclusive ? element : element.Parent);
		return value;
	}
}
