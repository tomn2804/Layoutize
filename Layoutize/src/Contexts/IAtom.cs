namespace Layoutize.Contexts;

public interface IAtom<T>
{
	bool TryGetValue(IBuildContext context, out T? value);
}
