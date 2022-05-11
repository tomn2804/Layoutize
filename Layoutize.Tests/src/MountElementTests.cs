using System;
using System.IO;
using System.Management.Automation;
using Xunit;

namespace Layoutize.Tests;

[Collection(nameof(WorkingDirectoryCollection))]
public abstract class MountElementTests : IDisposable
{
    private protected MountElementTests(WorkingDirectoryFixture fixture)
    {
        Shell = PowerShell.Create();
        WorkingDirectory = fixture.GetNewWorkingDirectory();
    }

    private protected PowerShell Shell { get; }

    private protected DirectoryInfo WorkingDirectory { get; }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        Shell.Dispose();
    }
}
