namespace Layoutize.Views;

internal interface IView : IViewContext
{
	void Create();

	void Delete();
}
