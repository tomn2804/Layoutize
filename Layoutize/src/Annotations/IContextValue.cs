namespace Layoutize.Annotations;

public interface IContextValue<T>
{
	bool TryGetValue(IBuildContext context, out T? value);
}
