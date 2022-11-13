using Layoutize.Elements;
using System;

namespace Layoutize.Contexts;

public static class Selector<T>
{
	public static T? GetValue(IBuildContext context, Type attributeType, bool inclusive = false)
	{
		T? value = default;
		void visitElement(Element? element)
		{
			if (
				element != null
				&& Attribute.GetCustomAttribute(element.Layout.GetType(), attributeType) is IAtom<T> attribute
				&& !attribute.TryGetValue(element, out value)
			)
			{
				visitElement(element.Parent);
			}
		}
		var element = context.Element;
		visitElement(inclusive ? element : element.Parent);
		return value;
	}
}
