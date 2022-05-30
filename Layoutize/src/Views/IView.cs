namespace Layoutize.Views;

internal interface IView
{
	void Create();

	void Delete();

	bool Exists { get; }

	string FullName { get; }

	string Name { get; }
}
