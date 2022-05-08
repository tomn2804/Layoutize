using Layoutize.Elements;
using System;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Reflection;
using System.Threading;
using Xunit;

namespace Layoutize.Tests
{
    [Collection("Working Directory Collection")]
    public class DirectoryLayoutTests
    {
        public DirectoryLayoutTests(WorkingDirectoryFixture fixture)
        {
            Fixture = fixture;
        }

        [Fact]
        public void MountElement_CreateDirectoryWithSingleChildDirectory_ReturnsContext()
        {
            using PowerShell shell = PowerShell.Create();

            DirectoryInfo workingDirectory = Fixture.GetNewWorkingDirectory();
            string layoutName = MethodBase.GetCurrentMethod().Name;

            using IBuildContext context = (IBuildContext)shell.AddScript($@"
                using module Layoutize
                using namespace Layoutize

                Mount-Element -Path '{workingDirectory.FullName}' -Layout (
                    [DirectoryLayout]@{{
                        Name = '{layoutName}'
                        Children = [DirectoryLayout]@{{ Name = '{layoutName}' }}
                    }}
                )
            ").Invoke().Last().BaseObject;

            string parentFullName = Path.Combine(workingDirectory.FullName, layoutName);
            string childFullName = Path.Combine(parentFullName, layoutName);

            Assert.True(Directory.Exists(parentFullName));
            Assert.True(Directory.Exists(childFullName));
            Assert.Single(Directory.GetFileSystemEntries(parentFullName));
        }

        [Fact]
        public void MountElement_CreateEmptyDirectory_ReturnsContext()
        {
            using PowerShell shell = PowerShell.Create();

            DirectoryInfo workingDirectory = Fixture.GetNewWorkingDirectory();
            string layoutName = MethodBase.GetCurrentMethod().Name;

            using IBuildContext context = (IBuildContext)shell.AddScript($@"
                using module Layoutize
                using namespace Layoutize

                Mount-Element -Path '{workingDirectory.FullName}' -Layout (
                    [DirectoryLayout]@{{ Name = '{layoutName}' }}
                )
            ").Invoke().Last().BaseObject;

            string fullName = Path.Combine(workingDirectory.FullName, layoutName);

            Assert.True(Directory.Exists(fullName));
            Assert.False(Directory.GetFileSystemEntries(fullName).Any());
        }

        private WorkingDirectoryFixture Fixture { get; }
    }

    [CollectionDefinition("Working Directory Collection")]
    public sealed class WorkingDirectoryCollection : ICollectionFixture<WorkingDirectoryFixture>
    {
    }

    public sealed class WorkingDirectoryFixture : IDisposable
    {
        public int _id;

        public void Dispose()
        {
            if (WorkingDirectory.Exists)
            {
                WorkingDirectory.Delete(true);
            }
        }

        public DirectoryInfo GetNewWorkingDirectory()
        {
            return WorkingDirectory.CreateSubdirectory(Interlocked.Increment(ref _id).ToString());
        }

        private readonly DirectoryInfo WorkingDirectory = Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), "LayoutizeTests"));
    }
}
