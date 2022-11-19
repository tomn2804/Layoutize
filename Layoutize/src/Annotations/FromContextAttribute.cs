namespace Layoutize.Annotations;

public class FromContextAttribute : ContextAttribute
{
	public FromContextAttribute(object key)
		: base(new ToContextAttribute(key))
	{
	}
}
