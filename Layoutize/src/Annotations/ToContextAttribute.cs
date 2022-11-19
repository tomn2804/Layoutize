namespace Layoutize.Annotations;

public class ToContextAttribute : ContextAttribute
{
	public ToContextAttribute(object key)
		: base(new FromContextAttribute(key))
	{
	}
}
