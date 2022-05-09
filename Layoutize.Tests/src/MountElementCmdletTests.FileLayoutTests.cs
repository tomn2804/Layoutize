using Layoutize.Elements;
using System.IO;
using System.Linq;
using System.Reflection;
using Xunit;

namespace Layoutize.Tests;

public partial class MountElementCmdletTests
{
    public sealed class FileLayoutTests : LayoutTests
    {
        public FileLayoutTests(WorkingDirectoryFixture fixture)
            : base(fixture)
        {
        }

        [Fact]
        public void MountElement_CreateEmptyFile_ReturnsContext()
        {
            var attributes = new { Name = $"{MethodBase.GetCurrentMethod().Name}.txt" };

            using IBuildContext context = (IBuildContext)Shell.AddScript($@"
                using module Layoutize
                using namespace Layoutize

                Mount-Element -Path '{WorkingDirectory.FullName}' -Layout (
                    [FileLayout]@{{ Name = '{attributes.Name}' }}
                )
            ").Invoke().Last().BaseObject;

            string fullName = Path.Combine(WorkingDirectory.FullName, attributes.Name);
            Assert.True(File.Exists(fullName));
            Assert.Empty(File.ReadLines(fullName));
        }
    }
}
