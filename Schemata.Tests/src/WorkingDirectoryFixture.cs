using System;
using System.IO;

namespace Schemata.Tests;

public sealed class WorkingDirectoryFixture : IDisposable
{
    public WorkingDirectoryFixture()
    {
        Directory.CreateDirectory(Path);
    }

    public static string Path => $"{System.IO.Path.GetTempPath()}Schemata.Tests";

    public void Dispose()
    {
        if (Directory.Exists(Path))
        {
            Directory.Delete(Path, true);
        }
    }
}
