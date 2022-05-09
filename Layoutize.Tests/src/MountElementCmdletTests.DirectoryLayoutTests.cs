using Layoutize.Elements;
using System.IO;
using System.Linq;
using System.Reflection;
using Xunit;

namespace Layoutize.Tests;

public partial class MountElementCmdletTests
{
    public sealed class DirectoryLayoutTests : LayoutTests
    {
        public DirectoryLayoutTests(WorkingDirectoryFixture fixture)
            : base(fixture)
        {
        }

        [Fact]
        public void MountElement_CreateDirectoryWithMultiChildDirectory_ReturnsContext()
        {
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

            using IBuildContext context = (IBuildContext)Shell.AddScript($@"
                using module Layoutize
                using namespace Layoutize

                Mount-Element -Path '{WorkingDirectory.FullName}' -Layout (
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

            string parentFullName = Path.Combine(WorkingDirectory.FullName, attributes.Name);
            Assert.True(Directory.Exists(parentFullName));
            Assert.Equal(attributes.Children.Length, Directory.GetFileSystemEntries(parentFullName).Length);

            Assert.All(attributes.Children, childAttributes =>
            {
                string childFullName = Path.Combine(parentFullName, childAttributes.Name);
                Assert.True(Directory.Exists(childFullName));
                Assert.Equal(childAttributes.Children.Length, Directory.GetFileSystemEntries(childFullName).Length);

                Assert.All(childAttributes.Children, grandChildAttributes =>
                {
                    string grandChildFullName = Path.Combine(childFullName, grandChildAttributes.Name);
                    Assert.True(Directory.Exists(grandChildFullName));
                    Assert.Empty(Directory.GetFileSystemEntries(grandChildFullName));
                });
            });
        }

        [Fact]
        public void MountElement_CreateDirectoryWithSingleChildDirectory_ReturnsContext()
        {
            var attributes = new
            {
                Name = MethodBase.GetCurrentMethod().Name,
                Children = new[] { new { Name = "1" } }
            };

            using IBuildContext context = (IBuildContext)Shell.AddScript($@"
                using module Layoutize
                using namespace Layoutize

                Mount-Element -Path '{WorkingDirectory.FullName}' -Layout (
                    [DirectoryLayout]@{{
                        Name = '{attributes.Name}'
                        Children = [DirectoryLayout]@{{ Name = '{attributes.Children[0].Name}' }}
                    }}
                )
            ").Invoke().Last().BaseObject;

            string parentFullName = Path.Combine(WorkingDirectory.FullName, attributes.Name);
            Assert.True(Directory.Exists(parentFullName));
            Assert.Equal(attributes.Children.Length, Directory.GetFileSystemEntries(parentFullName).Length);

            string childFullName = Path.Combine(parentFullName, attributes.Children[0].Name);
            Assert.True(Directory.Exists(childFullName));
            Assert.Empty(Directory.GetFileSystemEntries(childFullName));
        }

        [Fact]
        public void MountElement_CreateEmptyDirectory_ReturnsContext()
        {
            var attributes = new { Name = MethodBase.GetCurrentMethod().Name };

            using IBuildContext context = (IBuildContext)Shell.AddScript($@"
                using module Layoutize
                using namespace Layoutize

                Mount-Element -Path '{WorkingDirectory.FullName}' -Layout (
                    [DirectoryLayout]@{{ Name = '{attributes.Name}' }}
                )
            ").Invoke().Last().BaseObject;

            string fullName = Path.Combine(WorkingDirectory.FullName, attributes.Name);
            Assert.True(Directory.Exists(fullName));
            Assert.Empty(Directory.GetFileSystemEntries(fullName));
        }
    }
}
