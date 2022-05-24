using Xunit;

namespace Layoutize.Tests;

public abstract class LayoutTests<T> where T : Layout, new()
{
}

public abstract class ViewLayoutTests<T> : LayoutTests<T> where T : ViewLayout, new()
{
	[Fact]
	public void CreateView_MissingNameAttribute_ThrowsException()
	{
		void test()
		{
		}
		var layout = new T();
		var element = layout.CreateElement();
		Assert.Throws<InvalidOperationException>(() => layout.CreateView(element));
	}
}

public abstract class ViewGroupLayoutTests<T> : ViewLayoutTests<T> where T : ViewGroupLayout, new()
{
}

public class FileLayoutTests : ViewLayoutTests<FileLayout>
{
}

public class DirectoryLayoutTests : ViewLayoutTests<FileLayout>
{
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
