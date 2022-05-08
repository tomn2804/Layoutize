using Layoutize.Elements;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Reflection;
using Xunit;

namespace Layoutize.Tests
{
    [Collection(nameof(WorkingDirectoryCollection))]
    public class DirectoryLayoutTests
    {
        public DirectoryLayoutTests(WorkingDirectoryFixture fixture)
        {
            Fixture = fixture;
        }

        [Fact]
        public void MountElement_CreateDirectoryWithMultiChild_ReturnsContext()
        {
            using PowerShell shell = PowerShell.Create();

            DirectoryInfo workingDirectory = Fixture.GetNewWorkingDirectory();
            var attributes = new
            {
                Name = MethodBase.GetCurrentMethod().Name,
                Children = new[]
                {
                    new { Name = "1" },
                    new { Name = "2" },
                    new { Name = "3" }
                }
            };

            using IBuildContext context = (IBuildContext)shell.AddScript($@"
                using module Layoutize
                using namespace Layoutize

                Mount-Element -Path '{workingDirectory.FullName}' -Layout (
                    [DirectoryLayout]@{{
                        Name = '{attributes.Name}'
                        Children = @(
                            [DirectoryLayout]@{{ Name = '{attributes.Children[0].Name}' }},
                            [DirectoryLayout]@{{ Name = '{attributes.Children[1].Name}' }},
                            [DirectoryLayout]@{{ Name = '{attributes.Children[2].Name}' }}
                        )
                    }}
                )
            ").Invoke().Last().BaseObject;

            string parentFullName = Path.Combine(workingDirectory.FullName, attributes.Name);
            Assert.True(Directory.Exists(parentFullName));
            Assert.NotEmpty(Directory.GetFileSystemEntries(parentFullName));
            Assert.All(attributes.Children, childAttributes =>
            {
                string childFullName = Path.Combine(parentFullName, childAttributes.Name);
                Assert.True(Directory.Exists(childFullName));
                Assert.Empty(Directory.GetFileSystemEntries(childFullName));
            });
        }

        [Fact]
        public void MountElement_CreateDirectoryWithMultiLevelDepth_ReturnsContext()
        {
            using PowerShell shell = PowerShell.Create();

            DirectoryInfo workingDirectory = Fixture.GetNewWorkingDirectory();
            var attributes = new
            {
                Name = MethodBase.GetCurrentMethod().Name,
                Children = new[]
                {
                    new { Name = "1", Children = new[] { new { Name = "1.1" } } },
                    new { Name = "2", Children = new[] { new { Name = "2.1" } } },
                    new { Name = "3", Children = new[] { new { Name = "3.1" } } }
                }
            };

            using IBuildContext context = (IBuildContext)shell.AddScript($@"
                using module Layoutize
                using namespace Layoutize

                Mount-Element -Path '{workingDirectory.FullName}' -Layout (
                    [DirectoryLayout]@{{
                        Name = '{attributes.Name}'
                        Children = @(
                            [DirectoryLayout]@{{
                                Name = '{attributes.Children[0].Name}'
                                Children = [DirectoryLayout]@{{ Name = '{attributes.Children[0].Children[0].Name}' }}
                            }},
                            [DirectoryLayout]@{{
                                Name = '{attributes.Children[1].Name}'
                                Children = [DirectoryLayout]@{{ Name = '{attributes.Children[1].Children[0].Name}' }}
                            }},
                            [DirectoryLayout]@{{
                                Name = '{attributes.Children[2].Name}'
                                Children = [DirectoryLayout]@{{ Name = '{attributes.Children[2].Children[0].Name}' }}
                            }}
                        )
                    }}
                )
            ").Invoke().Last().BaseObject;

            string parentFullName = Path.Combine(workingDirectory.FullName, attributes.Name);
            Assert.True(Directory.Exists(parentFullName));
            Assert.All(attributes.Children, childAttributes =>
            {
                string childFullName = Path.Combine(parentFullName, childAttributes.Name);
                Assert.True(Directory.Exists(childFullName));
                Assert.NotEmpty(Directory.GetFileSystemEntries(childFullName));
                Assert.All(childAttributes.Children, grandChildAttributes =>
                {
                    string grandChildFullName = Path.Combine(childFullName, grandChildAttributes.Name);
                    Assert.True(Directory.Exists(grandChildFullName));
                    Assert.Empty(Directory.GetFileSystemEntries(grandChildFullName));
                });
            });
        }

        [Fact]
        public void MountElement_CreateDirectoryWithSingleChild_ReturnsContext()
        {
            using PowerShell shell = PowerShell.Create();

            DirectoryInfo workingDirectory = Fixture.GetNewWorkingDirectory();
            var attributes = new
            {
                Name = MethodBase.GetCurrentMethod().Name,
                Children = new[] { new { Name = "1" } }
            };

            using IBuildContext context = (IBuildContext)shell.AddScript($@"
                using module Layoutize
                using namespace Layoutize

                Mount-Element -Path '{workingDirectory.FullName}' -Layout (
                    [DirectoryLayout]@{{
                        Name = '{attributes.Name}'
                        Children = [DirectoryLayout]@{{ Name = '{attributes.Children[0].Name}' }}
                    }}
                )
            ").Invoke().Last().BaseObject;

            string parentFullName = Path.Combine(workingDirectory.FullName, attributes.Name);
            string childFullName = Path.Combine(parentFullName, attributes.Children[0].Name);

            Assert.True(Directory.Exists(parentFullName));
            Assert.True(Directory.Exists(childFullName));
            Assert.Single(Directory.GetFileSystemEntries(parentFullName));
        }

        [Fact]
        public void MountElement_CreateEmptyDirectory_ReturnsContext()
        {
            using PowerShell shell = PowerShell.Create();

            DirectoryInfo workingDirectory = Fixture.GetNewWorkingDirectory();
            var attributes = new { Name = MethodBase.GetCurrentMethod().Name };

            using IBuildContext context = (IBuildContext)shell.AddScript($@"
                using module Layoutize
                using namespace Layoutize

                Mount-Element -Path '{workingDirectory.FullName}' -Layout (
                    [DirectoryLayout]@{{ Name = '{attributes.Name}' }}
                )
            ").Invoke().Last().BaseObject;

            string fullName = Path.Combine(workingDirectory.FullName, attributes.Name);

            Assert.True(Directory.Exists(fullName));
            Assert.Empty(Directory.GetFileSystemEntries(fullName));
        }

        private WorkingDirectoryFixture Fixture { get; }
    }
}
