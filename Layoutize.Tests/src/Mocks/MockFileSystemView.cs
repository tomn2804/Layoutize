using Layoutize.Views;

namespace Layoutize.Tests.Mocks;

internal class MockFileSystemView : IView
{
	public MockFileSystemView(string fullName)
	{
		FullName = fullName;
	}

	public void Create()
	{
		Exists = true;
	}

	public void Delete()
	{
		Exists = false;
	}

	public bool Exists { get; private set; }

	public string FullName { get; }

	public string Name => Path.GetFileName(FullName);
}
