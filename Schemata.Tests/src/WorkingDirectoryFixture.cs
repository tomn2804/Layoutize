using System;
using System.IO;

namespace Schemata.Tests;

public sealed class WorkingDirectoryFixture : IDisposable
{
    private int ID { get; set; }

    public static string Path => $"{System.IO.Path.GetTempPath()}Schemata.Tests";

    public WorkingDirectoryFixture()
    {
        Directory.CreateDirectory(Path);
    }

    public string CreateUniqueWorkingDirectory()
    {
        return Directory.CreateDirectory(System.IO.Path.Combine(Path, (++ID).ToString())).FullName;
    }

    public void Dispose()
    {
        if (Directory.Exists(Path))
        {
            Directory.Delete(Path, true);
        }
    }
}
