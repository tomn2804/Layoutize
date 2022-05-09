using Layoutize.Elements;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Xunit;

namespace Layoutize.Tests;

public partial class MountElementCmdletTests
{
    public sealed class StatelessLayoutTests : LayoutTests
    {
        public StatelessLayoutTests(WorkingDirectoryFixture fixture)
            : base(fixture)
        {
        }

        [Fact]
        public void MountElement_CreateDirectoryWithMultiChildFilesAndDirectories_ReturnsContext()
        {
            Action<string> assertGrandChildDirectoryState = new(fullName =>
            {
                Assert.True(Directory.Exists(fullName));
                Assert.Empty(Directory.GetFileSystemEntries(fullName));
            });
            Action<string> assertGrandChildFileState = new(fullName =>
            {
                Assert.True(File.Exists(fullName));
                Assert.Empty(File.ReadLines(fullName));
            });
            var attributes = new
            {
                Name = MethodBase.GetCurrentMethod().Name,
                Children = new[]
                {
                    new {
                        Name = "1",
                        Children = new[]
                        {
                            new { Name = "1.1", AssertState = assertGrandChildDirectoryState },
                            new { Name = "1.2.txt", AssertState = assertGrandChildFileState }
                        }
                    },
                    new {
                        Name = "2",
                        Children = new[]
                        {
                            new { Name = "2.1", AssertState = assertGrandChildDirectoryState },
                            new { Name = "2.2.txt", AssertState = assertGrandChildFileState }
                        }
                    },
                    new {
                        Name = "3",
                        Children = new[]
                        {
                            new { Name = "3.1", AssertState = assertGrandChildDirectoryState },
                            new { Name = "3.2.txt", AssertState = assertGrandChildFileState }
                        }
                    }
                }
            };

            using IBuildContext context = (IBuildContext)Shell.AddScript($@"
                using module Layoutize
                using namespace Layoutize
                using namespace Layoutize.Elements
                using namespace System.Collections

                class TestChildFileLayout : StatelessLayout {{
                    TestChildFileLayout([IEnumerable]$attributes) : base($attributes) {{}}

                    [Layout]Build([IBuildContext]$context) {{
                        return [FileLayout]$this.Attributes
                    }}
                }}

                class TestChildDirectoryLayout : StatelessLayout {{
                    TestChildDirectoryLayout([IEnumerable]$attributes) : base($attributes) {{}}

                    [Layout]Build([IBuildContext]$context) {{
                        return [DirectoryLayout]$this.Attributes
                    }}
                }}

                class TestParentDirectoryLayout : StatelessLayout {{
                    TestParentDirectoryLayout([IEnumerable]$attributes) : base($attributes) {{}}

                    [Layout]Build([IBuildContext]$context) {{
                        return [DirectoryLayout]$this.Attributes.SetItem('Children', @(
                            [TestChildDirectoryLayout]@{{
                                Name = '{attributes.Children[0].Name}'
                                Children = @(
                                    [DirectoryLayout]@{{ Name = '{attributes.Children[0].Children[0].Name}' }},
                                    [TestChildFileLayout]@{{ Name = '{attributes.Children[0].Children[1].Name}' }}
                                )
                            }},
                            [TestChildDirectoryLayout]@{{
                                Name = '{attributes.Children[1].Name}'
                                Children = @(
                                    [DirectoryLayout]@{{ Name = '{attributes.Children[1].Children[0].Name}' }},
                                    [TestChildFileLayout]@{{ Name = '{attributes.Children[1].Children[1].Name}' }}
                                )
                            }},
                            [TestChildDirectoryLayout]@{{
                                Name = '{attributes.Children[2].Name}'
                                Children = @(
                                    [DirectoryLayout]@{{ Name = '{attributes.Children[2].Children[0].Name}' }},
                                    [TestChildFileLayout]@{{ Name = '{attributes.Children[2].Children[1].Name}' }}
                                )
                            }}
                        ))
                    }}
                }}

                Mount-Element -Path '{WorkingDirectory.FullName}' -Layout (
                    [TestParentDirectoryLayout]@{{ Name = '{attributes.Name}' }}
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
                    grandChildAttributes.AssertState(grandChildFullName);
                });
            });
        }

        [Fact]
        public void MountElement_CreateDirectoryWithSingleChildFile_ReturnsContext()
        {
            var attributes = new
            {
                Name = MethodBase.GetCurrentMethod().Name,
                Children = new[] { new { Name = "1.txt" } }
            };

            using IBuildContext context = (IBuildContext)Shell.AddScript($@"
                using module Layoutize
                using namespace Layoutize
                using namespace Layoutize.Elements
                using namespace System.Collections

                class TestFileLayout : StatelessLayout {{
                    TestFileLayout([IEnumerable]$attributes) : base($attributes) {{}}

                    [Layout]Build([IBuildContext]$context) {{
                        return [FileLayout]$this.Attributes
                    }}
                }}

                class TestDirectoryLayout : StatelessLayout {{
                    TestDirectoryLayout([IEnumerable]$attributes) : base($attributes) {{}}

                    [Layout]Build([IBuildContext]$context) {{
                        return [DirectoryLayout]$this.Attributes.SetItem('Children',
                            [TestFileLayout]@{{ Name = '{attributes.Children[0].Name}' }}
                        )
                    }}
                }}

                Mount-Element -Path '{WorkingDirectory.FullName}' -Layout (
                    [TestDirectoryLayout]@{{ Name = '{attributes.Name}' }}
                )
            ").Invoke().Last().BaseObject;

            string parentFullName = Path.Combine(WorkingDirectory.FullName, attributes.Name);
            Assert.True(Directory.Exists(parentFullName));
            Assert.Equal(attributes.Children.Length, Directory.GetFileSystemEntries(parentFullName).Length);

            string childFullName = Path.Combine(parentFullName, attributes.Children[0].Name);
            Assert.True(File.Exists(childFullName));
            Assert.Empty(File.ReadLines(childFullName));
        }

        [Fact]
        public void MountElement_CreateEmptyDirectory_ReturnsContext()
        {
            var attributes = new { Name = MethodBase.GetCurrentMethod().Name };

            using IBuildContext context = (IBuildContext)Shell.AddScript($@"
                using module Layoutize
                using namespace Layoutize
                using namespace Layoutize.Elements
                using namespace System.Collections

                class TestDirectoryLayout : StatelessLayout {{
                    TestDirectoryLayout([IEnumerable]$attributes) : base($attributes) {{}}

                    [Layout]Build([IBuildContext]$context) {{
                        return [DirectoryLayout]$this.Attributes
                    }}
                }}

                Mount-Element -Path '{WorkingDirectory.FullName}' -Layout (
                    [TestDirectoryLayout]@{{ Name = '{attributes.Name}' }}
                )
            ").Invoke().Last().BaseObject;

            string fullName = Path.Combine(WorkingDirectory.FullName, attributes.Name);
            Assert.True(Directory.Exists(fullName));
            Assert.Empty(Directory.GetFileSystemEntries(fullName));
        }

        [Fact]
        public void MountElement_CreateEmptyFile_ReturnsContext()
        {
            var attributes = new { Name = $"{MethodBase.GetCurrentMethod().Name}.txt" };

            using IBuildContext context = (IBuildContext)Shell.AddScript($@"
                using module Layoutize
                using namespace Layoutize
                using namespace Layoutize.Elements
                using namespace System.Collections

                class TestFileLayout : StatelessLayout {{
                    TestFileLayout([IEnumerable]$attributes) : base($attributes) {{}}

                    [Layout]Build([IBuildContext]$context) {{
                        return [FileLayout]$this.Attributes
                    }}
                }}

                Mount-Element -Path '{WorkingDirectory.FullName}' -Layout (
                    [TestFileLayout]@{{ Name = '{attributes.Name}' }}
                )
            ").Invoke().Last().BaseObject;

            string fullName = Path.Combine(WorkingDirectory.FullName, attributes.Name);
            Assert.True(File.Exists(fullName));
            Assert.Empty(File.ReadLines(fullName));
        }
    }
}
