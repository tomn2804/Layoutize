namespace Layoutize.Contexts;

public interface IContextValue<T>
{
	bool TryGetValue(IBuildContext context, out T? value);
}
