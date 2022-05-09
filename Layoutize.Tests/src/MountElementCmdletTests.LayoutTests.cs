using System;
using System.IO;
using System.Management.Automation;
using Xunit;

namespace Layoutize.Tests;

public partial class MountElementCmdletTests
{
    [Collection(nameof(WorkingDirectoryCollection))]
    public abstract class LayoutTests : IDisposable
    {
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Shell.Dispose();
        }

        private protected LayoutTests(WorkingDirectoryFixture fixture)
        {
            Shell = PowerShell.Create();
            WorkingDirectory = fixture.GetNewWorkingDirectory();
        }

        private protected PowerShell Shell { get; }

        private protected DirectoryInfo WorkingDirectory { get; }
    }

}
