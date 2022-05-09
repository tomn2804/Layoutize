using Layoutize.Elements;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Xunit;

namespace Layoutize.Tests;

public partial class MountElementCmdletTests
{
    public sealed class StatefulLayoutTests : LayoutTests
    {
        public StatefulLayoutTests(WorkingDirectoryFixture fixture)
            : base(fixture)
        {
        }

        [Fact]
        public void MountElement_CreateDirectoryWithMultiChildFilesAndDirectoriesWithUpdatableChildren_ReturnsContext()
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
                InitialChildren = new[]
                {
                    new {
                        Name = "1",
                        DeleteOnUnMount = true,
                        Children = new[]
                        {
                            new { Name = "1.1", DeleteOnUnMount = false, AssertState = assertGrandChildDirectoryState },
                            new { Name = "1.2.txt", DeleteOnUnMount = false, AssertState = assertGrandChildFileState }
                        }
                    },
                    new {
                        Name = "2",
                        DeleteOnUnMount = false,
                        Children = new[]
                        {
                            new { Name = "2.1", DeleteOnUnMount = true, AssertState = assertGrandChildDirectoryState },
                            new { Name = "2.2.txt", DeleteOnUnMount = false, AssertState = assertGrandChildFileState }
                        }
                    },
                    new {
                        Name = "3",
                        DeleteOnUnMount = false,
                        Children = new[]
                        {
                            new { Name = "3.1", DeleteOnUnMount = false, AssertState = assertGrandChildDirectoryState },
                            new { Name = "3.2.txt", DeleteOnUnMount = true, AssertState = assertGrandChildFileState }
                        }
                    }
                },
                NewChildren = new[]
                {
                    new {
                        Name = "2",
                        Children = new[]
                        {
                            new { Name = "2.1", AssertState = assertGrandChildDirectoryState },
                            new { Name = "2.2.txt", AssertState = assertGrandChildFileState },
                            new { Name = "2.3.txt", AssertState = assertGrandChildFileState }
                        }
                    },
                    new {
                        Name = "3",
                        Children = new[]
                        {
                            new { Name = "3.1", AssertState = assertGrandChildDirectoryState },
                            new { Name = "3.2.txt", AssertState = assertGrandChildFileState },
                            new { Name = "3.3.txt", AssertState = assertGrandChildFileState }
                        }
                    }
                }
            };

            dynamic layout = Shell.AddScript($@"
                using module Layoutize
                using namespace Layoutize
                using namespace Layoutize.Elements
                using namespace System.Collections

                class TestChildDirectoryState : State {{
                    TestChildDirectoryState([TestChildDirectoryLayout]$layout) : base($layout) {{}}

                    [Layout]Build([IBuildContext]$context) {{
                        return [DirectoryLayout]$this.Attributes
                    }}
                }}

                class TestChildDirectoryLayout : StatefulLayout {{
                    [TestChildDirectoryState]$State

                    TestChildDirectoryLayout([IEnumerable]$attributes) : base($attributes) {{}}

                    [State]CreateState() {{
                        return [TestChildDirectoryState]::new($this)
                    }}
                }}

                class TestChildFileState : State {{
                    TestChildFileState([TestChildFileLayout]$layout) : base($layout) {{}}

                    [Layout]Build([IBuildContext]$context) {{
                        return [FileLayout]$this.Attributes
                    }}
                }}

                class TestChildFileLayout : StatefulLayout {{
                    [TestChildFileState]$State

                    TestChildFileLayout([IEnumerable]$attributes) : base($attributes) {{}}

                    [State]CreateState() {{
                        return [TestChildFileState]::new($this)
                    }}
                }}

                class TestParentDirectoryState : State {{
                    [Layout[]]$Children = @(
                        [TestChildDirectoryLayout]@{{
                            Name = '{attributes.InitialChildren[0].Name}'
                            DeleteOnUnmount = {(attributes.InitialChildren[0].DeleteOnUnMount ? "$true" : "$false")}
                            Children = @(
                                [DirectoryLayout]@{{
                                    Name = '{attributes.InitialChildren[0].Children[0].Name}'
                                    DeleteOnUnmount = {(attributes.InitialChildren[0].Children[1].DeleteOnUnMount ? "$true" : "$false")}
                                }},
                                [TestChildFileLayout]@{{
                                    Name = '{attributes.InitialChildren[0].Children[1].Name}'
                                    DeleteOnUnmount = {(attributes.InitialChildren[0].Children[1].DeleteOnUnMount ? "$true" : "$false")}
                                }}
                            )
                        }},
                        [TestChildDirectoryLayout]@{{
                            Name = '{attributes.InitialChildren[1].Name}'
                            DeleteOnUnmount = {(attributes.InitialChildren[1].DeleteOnUnMount ? "$true" : "$false")}
                            Children = @(
                                [DirectoryLayout]@{{
                                    Name = '{attributes.InitialChildren[1].Children[0].Name}'
                                    DeleteOnUnmount = {(attributes.InitialChildren[1].Children[1].DeleteOnUnMount ? "$true" : "$false")}
                                }},
                                [TestChildFileLayout]@{{
                                    Name = '{attributes.InitialChildren[1].Children[1].Name}'
                                    DeleteOnUnmount = {(attributes.InitialChildren[1].Children[1].DeleteOnUnMount ? "$true" : "$false")}
                                }}
                            )
                        }},
                        [TestChildDirectoryLayout]@{{
                            Name = '{attributes.InitialChildren[2].Name}'
                            DeleteOnUnmount = {(attributes.InitialChildren[2].DeleteOnUnMount ? "$true" : "$false")}
                            Children = @(
                                [DirectoryLayout]@{{
                                    Name = '{attributes.InitialChildren[2].Children[0].Name}'
                                    DeleteOnUnmount = {(attributes.InitialChildren[2].Children[1].DeleteOnUnMount ? "$true" : "$false")}
                                }},
                                [TestChildFileLayout]@{{
                                    Name = '{attributes.InitialChildren[2].Children[1].Name}'
                                    DeleteOnUnmount = {(attributes.InitialChildren[2].Children[1].DeleteOnUnMount ? "$true" : "$false")}
                                }}
                            )
                        }}
                    )

                    TestParentDirectoryState([TestParentDirectoryLayout]$layout) : base($layout) {{}}

                    [Layout]Build([IBuildContext]$context) {{
                        return [DirectoryLayout]$this.Attributes.SetItem('Children', $this.Children)
                    }}

                    UpdateChildren() {{
                        $this.SetState(@{{
                            Children = [Layout[]]@(
                                [TestChildDirectoryLayout]@{{
                                    Name = '{attributes.NewChildren[0].Name}'
                                    Children = @(
                                        [DirectoryLayout]@{{ Name = '{attributes.NewChildren[0].Children[0].Name}' }},
                                        [TestChildFileLayout]@{{ Name = '{attributes.NewChildren[0].Children[1].Name}' }},
                                        [TestChildFileLayout]@{{ Name = '{attributes.NewChildren[0].Children[2].Name}' }}
                                    )
                                }},
                                [TestChildDirectoryLayout]@{{
                                    Name = '{attributes.NewChildren[1].Name}'
                                    Children = @(
                                        [DirectoryLayout]@{{ Name = '{attributes.NewChildren[1].Children[0].Name}' }},
                                        [TestChildFileLayout]@{{ Name = '{attributes.NewChildren[1].Children[1].Name}' }},
                                        [TestChildFileLayout]@{{ Name = '{attributes.NewChildren[1].Children[2].Name}' }}
                                    )
                                }}
                            )
                        }})
                    }}
                }}

                class TestParentDirectoryLayout : StatefulLayout {{
                    [TestParentDirectoryState]$State

                    TestParentDirectoryLayout([IEnumerable]$attributes) : base($attributes) {{}}

                    [State]CreateState() {{
                        $this.State = [TestParentDirectoryState]::new($this)
                        return $this.State
                    }}
                }}

                $layout = [TestParentDirectoryLayout]@{{ Name = '{attributes.Name}' }}
                Mount-Element -Path '{WorkingDirectory.FullName}' -Layout $layout
                $layout
            ").Invoke().Last().BaseObject;

            string parentFullName = Path.Combine(WorkingDirectory.FullName, attributes.Name);
            Assert.True(Directory.Exists(parentFullName));
            Assert.Equal(attributes.InitialChildren.Length, Directory.GetFileSystemEntries(parentFullName).Length);

            Assert.All(attributes.InitialChildren, childAttributes =>
            {
                string initialChildFullName = Path.Combine(parentFullName, childAttributes.Name);
                Assert.True(Directory.Exists(initialChildFullName));
                Assert.Equal(childAttributes.Children.Length, Directory.GetFileSystemEntries(initialChildFullName).Length);

                Assert.All(childAttributes.Children, grandChildAttributes =>
                {
                    string initialGrandChildFullName = Path.Combine(initialChildFullName, grandChildAttributes.Name);
                    grandChildAttributes.AssertState(initialGrandChildFullName);
                });
            });

            layout.State.UpdateChildren();
            Assert.True(Directory.Exists(parentFullName));
            Assert.False(Directory.Exists(Path.Combine(parentFullName, attributes.InitialChildren[0].Name)));

            Assert.Equal(attributes.NewChildren.Length, Directory.GetFileSystemEntries(parentFullName).Length);
            Assert.All(attributes.NewChildren, childAttributes =>
            {
                string newChildFullName = Path.Combine(parentFullName, childAttributes.Name);
                Assert.True(Directory.Exists(newChildFullName));
                Assert.Equal(childAttributes.Children.Length, Directory.GetFileSystemEntries(newChildFullName).Length);

                Assert.All(childAttributes.Children, grandChildAttributes =>
                {
                    string newGrandChildFullName = Path.Combine(newChildFullName, grandChildAttributes.Name);
                    grandChildAttributes.AssertState(newGrandChildFullName);
                });
            });
        }

        [Fact]
        public void MountElement_CreateDirectoryWithSingleChildFileWithUpdatableChildren_ReturnsContext()
        {
            var attributes = new
            {
                Name = MethodBase.GetCurrentMethod().Name,
                InitialChildren = new[] { new { Name = "1.txt", DeleteOnUnmount = true } },
                NewChildren = new[] { new { Name = "2.txt" } }
            };

            dynamic layout = Shell.AddScript($@"
                using module Layoutize
                using namespace Layoutize
                using namespace Layoutize.Elements
                using namespace System.Collections

                class TestChildFileState : State {{
                    TestChildFileState([TestChildFileLayout]$layout) : base($layout) {{}}

                    [Layout]Build([IBuildContext]$context) {{
                        return [FileLayout]$this.Attributes
                    }}
                }}

                class TestChildFileLayout : StatefulLayout {{
                    [TestChildFileState]$State

                    TestChildFileLayout([IEnumerable]$attributes) : base($attributes) {{}}

                    [State]CreateState() {{
                        return [TestChildFileState]::new($this)
                    }}
                }}

                class TestParentDirectoryState : State {{
                    [Layout]$Children = [TestChildFileLayout]@{{
                        Name = '{attributes.InitialChildren[0].Name}'
                        DeleteOnUnmount = {(attributes.InitialChildren[0].DeleteOnUnmount ? "$true" : "$false")}
                    }}

                    TestParentDirectoryState([TestParentDirectoryLayout]$layout) : base($layout) {{}}

                    [Layout]Build([IBuildContext]$context) {{
                        return [DirectoryLayout]$this.Attributes.SetItem('Children', $this.Children)
                    }}

                    UpdateChildren() {{
                        $this.SetState(@{{
                            Children = [TestChildFileLayout]@{{
                                Name = '{attributes.NewChildren[0].Name}'
                            }}
                        }})
                    }}
                }}

                class TestParentDirectoryLayout : StatefulLayout {{
                    [TestParentDirectoryState]$State

                    TestParentDirectoryLayout([IEnumerable]$attributes) : base($attributes) {{}}

                    [State]CreateState() {{
                        $this.State = [TestParentDirectoryState]::new($this)
                        return $this.State
                    }}
                }}

                $layout = [TestParentDirectoryLayout]@{{ Name = '{attributes.Name}' }}
                Mount-Element -Path '{WorkingDirectory.FullName}' -Layout $layout
                $layout
            ").Invoke().Last().BaseObject;

            string parentFullName = Path.Combine(WorkingDirectory.FullName, attributes.Name);
            Assert.True(Directory.Exists(parentFullName));
            Assert.Equal(attributes.InitialChildren.Length, Directory.GetFileSystemEntries(parentFullName).Length);

            string initialChildFullName = Path.Combine(parentFullName, attributes.InitialChildren[0].Name);
            Assert.True(File.Exists(initialChildFullName));
            Assert.Empty(File.ReadLines(initialChildFullName));

            layout.State.UpdateChildren();
            Assert.True(Directory.Exists(parentFullName));
            Assert.False(File.Exists(initialChildFullName));

            string newChildFullName = Path.Combine(parentFullName, attributes.NewChildren[0].Name);
            Assert.True(File.Exists(newChildFullName));
            Assert.Empty(File.ReadLines(newChildFullName));
            Assert.Equal(attributes.NewChildren.Length, Directory.GetFileSystemEntries(parentFullName).Length);
        }

        [Fact]
        public void MountElement_CreateEmptyDirectoryWithUpdatableName_ReturnsContext()
        {
            var attributes = new
            {
                InitialName = $"{MethodBase.GetCurrentMethod().Name}",
                NewName = $"{MethodBase.GetCurrentMethod().Name}.New"
            };

            dynamic layout = Shell.AddScript($@"
                using module Layoutize
                using namespace Layoutize
                using namespace Layoutize.Elements
                using namespace System.Collections

                class TestDirectoryState : State {{
                    [string]$Name = '{attributes.InitialName}'

                    TestDirectoryState([TestDirectoryLayout]$layout) : base($layout) {{}}

                    [Layout]Build([IBuildContext]$context) {{
                        return [DirectoryLayout]$this.Attributes.SetItem('Name', $this.Name)
                    }}

                    UpdateName() {{
                        $this.SetState(@{{ Name = '{attributes.NewName}' }})
                    }}
                }}

                class TestDirectoryLayout : StatefulLayout {{
                    [TestDirectoryState]$State

                    TestDirectoryLayout([IEnumerable]$attributes) : base($attributes) {{}}

                    [State]CreateState() {{
                        $this.State = [TestDirectoryState]::new($this)
                        return $this.State
                    }}
                }}

                $layout = [TestDirectoryLayout]@{{}}
                Mount-Element -Path '{WorkingDirectory.FullName}' -Layout $layout
                $layout
            ").Invoke().Last().BaseObject;

            string initialFullName = Path.Combine(WorkingDirectory.FullName, attributes.InitialName);
            Assert.True(Directory.Exists(initialFullName));
            Assert.Empty(Directory.GetFileSystemEntries(initialFullName));

            layout.State.UpdateName();
            Assert.False(Directory.Exists(initialFullName));

            string newFullName = Path.Combine(WorkingDirectory.FullName, attributes.NewName);
            Assert.True(Directory.Exists(newFullName));
            Assert.Empty(Directory.GetFileSystemEntries(newFullName));
        }

        [Fact]
        public void MountElement_CreateEmptyFileWithUpdatableName_ReturnsContext()
        {
            var attributes = new
            {
                InitialName = $"{MethodBase.GetCurrentMethod().Name}.txt",
                NewName = $"{MethodBase.GetCurrentMethod().Name}.New.txt"
            };

            dynamic layout = Shell.AddScript($@"
                using module Layoutize
                using namespace Layoutize
                using namespace Layoutize.Elements
                using namespace System.Collections

                class TestFileState : State {{
                    [string]$Name = '{attributes.InitialName}'

                    TestFileState([TestFileLayout]$layout) : base($layout) {{}}

                    [Layout]Build([IBuildContext]$context) {{
                        return [FileLayout]$this.Attributes.SetItem('Name', $this.Name)
                    }}

                    UpdateName() {{
                        $this.SetState(@{{ Name = '{attributes.NewName}' }})
                    }}
                }}

                class TestFileLayout : StatefulLayout {{
                    [TestFileState]$State

                    TestFileLayout([IEnumerable]$attributes) : base($attributes) {{}}

                    [State]CreateState() {{
                        $this.State = [TestFileState]::new($this)
                        return $this.State
                    }}
                }}

                $layout = [TestFileLayout]@{{}}
                Mount-Element -Path '{WorkingDirectory.FullName}' -Layout $layout
                $layout
            ").Invoke().Last().BaseObject;

            string initialFullName = Path.Combine(WorkingDirectory.FullName, attributes.InitialName);
            Assert.True(File.Exists(initialFullName));
            Assert.Empty(File.ReadLines(initialFullName));

            layout.State.UpdateName();
            Assert.False(File.Exists(initialFullName));

            string newFullName = Path.Combine(WorkingDirectory.FullName, attributes.NewName);
            Assert.True(File.Exists(newFullName));
            Assert.Empty(File.ReadLines(newFullName));
        }
    }
}
