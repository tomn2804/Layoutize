namespace Layoutize.Views;

internal interface IViewContext
{
	bool Exists { get; }

	string FullName { get; }

	string Name { get; }
}
