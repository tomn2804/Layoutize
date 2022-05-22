using Xunit;

namespace Layoutize.Tests;

public class DirectoryLayoutTests : LayoutTests
{
    protected override Layout CreateLayout()
    {
        return new FileLayout() { Name = "FileLayoutTests" };
    }
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

public class DirectoryViewTests : ViewTests
{
}
