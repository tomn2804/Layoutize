using Layoutize.Layouts;
using Xunit;

namespace Layoutize.Tests;

public abstract class LayoutTests<T> where T : Layout, new()
{
}

public abstract class ViewLayoutTests<T> : LayoutTests<T> where T : ViewLayout, new()
{
	[Fact]
	public void InitInherit_MergeAndOverrideNames_ReturnsDerivedLayout()
	{
		var baseLayout = new T { Name = nameof(ViewLayoutTests<T>) };
		var derivedLayout = new T { Inherit = baseLayout, Name = "derivedLayout" };
		Assert.Equal(nameof(ViewLayoutTests<T>), baseLayout.Name);
		Assert.Equal("derivedLayout", derivedLayout.Name);
	}

	[Fact]
	public void InitInherit_MergeTwoLayoutNames_ReturnsDerivedLayout()
	{
		var baseLayout = new T { Name = nameof(ViewLayoutTests<T>) };
		var derivedLayout = new T { Inherit = baseLayout };
		Assert.Equal(baseLayout.Name, derivedLayout.Name);
	}
}

public abstract class ViewGroupLayoutTests<T> : ViewLayoutTests<T> where T : ViewGroupLayout, new()
{
	[Fact]
	public void InitInherit_MergeTwoLayoutChildren_ReturnsDerivedLayout()
	{
		var children = Enumerable.Repeat(new T { Name = nameof(ViewLayoutTests<T>) }, 3);
		var baseLayout = new T { Name = nameof(ViewLayoutTests<T>), Children = children };
		var derivedLayout = new T { Inherit = baseLayout };
		Assert.Equal(baseLayout.Name, derivedLayout.Name);
		Assert.Equal(baseLayout.Children, derivedLayout.Children);
	}
}

public class FileLayoutTests : ViewLayoutTests<FileLayout>
{
}

public class DirectoryLayoutTests : ViewGroupLayoutTests<DirectoryLayout>
{
}
