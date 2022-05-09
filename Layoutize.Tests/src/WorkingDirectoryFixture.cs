using System;
using System.IO;
using System.Threading;

namespace Layoutize.Tests;

public sealed class WorkingDirectoryFixture : IDisposable
{
    private readonly DirectoryInfo WorkingDirectory = new(Path.Combine(Path.GetTempPath(), "LayoutizeTests"));

    private int _id;

    public WorkingDirectoryFixture()
    {
        if (WorkingDirectory.Exists)
        {
            WorkingDirectory.Delete(true);
            WorkingDirectory.Create();
        }
    }

    public void Dispose()
    {
        WorkingDirectory.Delete(true);
    }

    internal DirectoryInfo GetNewWorkingDirectory()
    {
        return WorkingDirectory.CreateSubdirectory(Interlocked.Increment(ref _id).ToString());
    }
}
