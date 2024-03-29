﻿using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using Layoutize.Layouts;
using Xunit;

namespace Layoutize.Tests;

public abstract class LayoutTests<T> where T : Layout, new()
{
	[Fact]
	public void InitInherit_MergeWithInvalidLayout_ThrowsValidationException()
	{
		Assert.Throws<ValidationException>(() => new T { Inherit = InvalidLayout });
	}

	public abstract Layout InvalidLayout { get; }
}

public abstract class ViewLayoutTests<T> : LayoutTests<T> where T : FileSystemLayout, new()
{
	public static IEnumerable<object?[]> InvalidNames => Path.GetInvalidFileNameChars()
		.Select(name => new object[] { name })
		.Append(new object?[] { null });

	[Fact]
	public void InitInherit_MergeAndOverrideProperties_ReturnsDerivedLayout()
	{
		const string baseName = nameof(baseName);
		const string derivedName = nameof(derivedName);

		var baseLayout = new T { Name = baseName };
		var derivedLayout = new T { Inherit = baseLayout, Name = derivedName };

		Assert.Equal(baseName, baseLayout.Name);
		Assert.Equal(derivedName, derivedLayout.Name);
	}

	[Fact]
	public void InitInherit_MergeTwoLayouts_ReturnsDerivedLayout()
	{
		const string baseName = nameof(baseName);

		var baseLayout = new T { Name = baseName };
		var derivedLayout = new T { Inherit = baseLayout };

		Assert.Equal(baseLayout.Name, derivedLayout.Name);
	}

	public override T InvalidLayout => new();
}

public abstract class ViewGroupLayoutTests<T> : ViewLayoutTests<T> where T : DirectoryLayout, new()
{
	[Fact]
	public void InitInherit_MergeAndOverrideEnumerableProperties_ReturnsDerivedLayout()
	{
		const string baseName = nameof(baseName);
		const string derivedName = nameof(derivedName);

		var baseChildren = Enumerable.Repeat(new T { Name = baseName }, 2).ToImmutableList();
		var derivedChildren = Enumerable.Repeat(new T { Name = derivedName }, 3).ToImmutableList();

		var baseLayout = new T { Name = baseName, Children = baseChildren };
		var derivedLayout = new T { Inherit = baseLayout, Children = derivedChildren };

		Assert.Equal(baseLayout.Name, derivedLayout.Name);
		Assert.Equal(baseChildren, baseLayout.Children);
		Assert.Equal(derivedChildren, derivedLayout.Children);
	}
}

public class FileLayoutTests : ViewLayoutTests<FileLayout>
{
}

public class DirectoryLayoutTests : ViewGroupLayoutTests<DirectoryLayout>
{
}
