using Xunit;

namespace Layoutize.Tests;

public abstract class LayoutTests
{
}

public class FileLayoutTests : LayoutTests
{
	[Fact]
	public void CreateView_MissingNameAttribute_ThrowsException()
	{
		var layout = new FileLayout();
		var element = layout.CreateElement();
		Assert.Throws<InvalidOperationException>(() => layout.CreateView(element));
	}
}

/*
internal class MockDirectoryView : DirectoryView
{
	internal MockDirectoryView(DirectoryView view)
		: base(new(view.FullName))
	{
		FullName = view.FullName;
	}

	internal override void Create()
	{
		_exists = true;
	}

	internal override void Delete()
	{
		_exists = false;
	}

	internal override bool Exists => _exists;

	internal override string FullName { get; }

	internal override string Name => Path.GetFileName(FullName);

	private bool _exists;
}

internal class MockFileView : FileView
{
	internal MockFileView(FileView view)
		: base(new(view.FullName))
	{
		FullName = view.FullName;
	}

	internal override void Create()
	{
		_exists = true;
	}

	internal override void Delete()
	{
		_exists = false;
	}

	internal override bool Exists => _exists;

	internal override string FullName { get; }

	internal override string Name => Path.GetFileName(FullName);

	private bool _exists;
}

public class MockFileLayout : FileLayout
{
	internal override MockFileView CreateView(IBuildContext context)
	{
		return new (base.CreateView(context));
	}
}
*/
