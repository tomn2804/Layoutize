using Layoutize.Elements;
using Layoutize.Views;
using System.IO;
using Xunit;

namespace Layoutize.Tests;

public class DirectoryLayoutTests : LayoutTests
{
    protected override Layout CreateLayout()
    {
        return new FileLayout() { Name = "FileLayoutTests" };
    }
}

public class DirectoryViewTests : ViewTests
{
}

public class FileLayoutTests : LayoutTests
{
    protected override Layout CreateLayout()
    {
        return new FileLayout() { Name = "FileLayoutTests" };
    }
}

public class FileViewTests : ViewTests
{
    [Fact]
    public void Create_Basic_FileExists()
    {
        _ = new MockFileLayout() { Name = "Main.cs" };
    }
}

public abstract class LayoutTests
{
    [Fact]
    public void CreateElement_Basic_ReturnsElement()
    {
        _ = CreateLayout();
    }

    protected abstract Layout CreateLayout();
}

public class MockDirectoryLayout : DirectoryLayout
{
    internal override MockDirectoryView CreateView(IBuildContext context)
    {
        return new(base.CreateView(context));
    }
}

public class StatefulLayoutTests : LayoutTests
{
    protected override Layout CreateLayout()
    {
        return new FileLayout() { Name = "FileLayoutTests" };
    }
}

public class StatelessLayoutTests : LayoutTests
{
    protected override Layout CreateLayout()
    {
        return new FileLayout() { Name = "FileLayoutTests" };
    }
}

public abstract class ViewTests
{
}

internal class MockDirectoryView : DirectoryView
{
    private readonly string _fullName;

    private bool _exists;

    internal MockDirectoryView(DirectoryView view)
        : base(new(view.FullName))
    {
        _fullName = view.FullName;
    }

    internal override bool Exists => _exists;

    internal override string FullName => _fullName;

    internal override string Name => Path.GetFileName(FullName);

    internal override void Create()
    {
        _exists = true;
    }

    internal override void Delete()
    {
        _exists = false;
    }
}

internal class MockFileView : FileView
{
    private readonly string _fullName;

    private bool _exists;

    internal MockFileView(FileView view)
        : base(new(view.FullName))
    {
        _fullName = view.FullName;
    }

    internal override bool Exists => _exists;

    internal override string FullName => _fullName;

    internal override string Name => Path.GetFileName(FullName);

    internal override void Create()
    {
        _exists = true;
    }

    internal override void Delete()
    {
        _exists = false;
    }
}

public class MockFileLayout : FileLayout
{
    internal override MockFileView CreateView(IBuildContext context)
    {
        return new(base.CreateView(context));
    }
}
