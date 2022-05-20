namespace Layoutize.Views;

internal abstract class View
{
    internal abstract bool Exists { get; }

    internal abstract string FullName { get; }

    internal abstract string Name { get; }

    internal abstract void Create();

    internal abstract void Delete();
}
