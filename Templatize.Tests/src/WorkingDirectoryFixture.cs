using System;
using System.IO;

namespace Templatize.Tests;

public sealed class WorkingDirectoryFixture : IDisposable
{
    public WorkingDirectoryFixture()
    {
        Directory.CreateDirectory(Path);
    }

    public static string Path => $"{System.IO.Path.GetTempPath()}Templatize.Tests";

    public void Dispose()
    {
        if (Directory.Exists(Path))
        {
            Directory.Delete(Path, true);
        }
    }
}
