using Layoutize.Elements;
using Layoutize.Layouts;
using Layoutize.Views;

namespace Layoutize.Tests.Mocks;

internal class MockDirectoryLayout : DirectoryLayout
{
	internal override IView CreateView(IBuildContext context)
	{
		return new MockFileSystemView(base.CreateView(context).FullName);
	}
}
