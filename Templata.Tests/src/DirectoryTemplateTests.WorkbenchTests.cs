using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Reflection;
using Xunit;

namespace Templata.Tests;

public sealed partial class DirectoryTemplateTests
{
    [Collection("Working directory")]
    public sealed partial class WorkbenchTests
    {
        public WorkbenchTests(WorkingDirectoryFixture fixture)
        {
            Fixture = fixture;
        }

        [Theory, MemberData(nameof(InvalidData.NonNullNames), MemberType = typeof(InvalidData))]
        public void Build_WithInvalidNonNullName_ThrowsException(string name)
        {
            Dictionary<object, object> details = new() { { Template.DetailOption.Name, name } };
            DirectoryTemplate template = new(details);
            View.Factory workbench = new(template);
            Assert.Throws<ArgumentException>("context", () =>
            {
                try
                {
                    workbench.Build();
                }
                catch (TargetInvocationException e)
                {
                    throw e.InnerException;
                }
            });
        }

        [Theory, MemberData(nameof(InvalidData.NullNames), MemberType = typeof(InvalidData))]
        public void Build_WithInvalidNullName_ThrowsException(string name)
        {
            Dictionary<object, object> details = new() { { Template.DetailOption.Name, name } };
            DirectoryTemplate template = new(details);
            View.Factory workbench = new(template);
            Assert.Throws<ArgumentNullException>("context", () =>
            {
                try
                {
                    workbench.Build();
                }
                catch (TargetInvocationException e)
                {
                    throw e.InnerException;
                }
            });
        }

        [Theory]
        [InlineData("Test")]
        [InlineData("Test.txt")]
        public void BuildTo_WorkingDirectoryFromDynamicComposition_ReturnsView(string viewName)
        {
            using PowerShell instance = PowerShell.Create();

            int replication = 3;
            string templateName = nameof(templateName);
            string workingDirectoryPath = GetWorkingDirectory(nameof(BuildTo_WorkingDirectoryFromDynamicComposition_ReturnsView));

            IEnumerable<PSObject> results = instance.AddScript($@"
                using module Templata
                using namespace Templata
                using namespace System.Collections

                class {templateName} : Template[DirectoryView] {{
                    {templateName}([IDictionary]$details) : base($details) {{}}

                    [Context]ToBlueprint() {{
                        return [DirectoryTemplate]$this.Details
                    }}
                }}

                $workbench = [View+Workbench]::new([{templateName}]@{{ [Template+DetailOption]::Name = '{viewName}' }})

                1..{replication} | ForEach-Object -Process {{
                    New-Item -Path '{workingDirectoryPath}' -Name $_ -ItemType 'Directory' | Out-Null
                    $workbench.BuildTo(""{workingDirectoryPath}\$_"")
                }}
            ").Invoke().TakeLast(replication);

            Assert.All(Enumerable.Range(1, replication), i =>
            {
                Assert.True(Directory.Exists($"{workingDirectoryPath}\\{i}\\{viewName}"));
                Assert.False(Directory.EnumerateFileSystemEntries($"{workingDirectoryPath}\\{i}\\{viewName}").Any());
            });
            Assert.All(results, result => Assert.IsType<DirectoryView>(result.BaseObject));
        }

        [Fact]
        public void BuildTo_WorkingDirectoryFromDynamicCompositionWithAChildFile_ReturnsView()
        {
            using PowerShell instance = PowerShell.Create();

            string workingDirectoryPath = GetWorkingDirectory(nameof(BuildTo_WorkingDirectoryFromDynamicCompositionWithAChildFile_ReturnsView));

            string parentTemplateName, parentViewName;
            parentTemplateName = parentViewName = nameof(parentTemplateName);

            string childTemplateName, childViewName;
            childTemplateName = childViewName = nameof(childTemplateName);

            DirectoryView result = (DirectoryView)instance.AddScript($@"
                using module Templata
                using namespace Templata
                using namespace System.Collections

                class {parentTemplateName} : Template[DirectoryView] {{
                    {parentTemplateName}([IDictionary]$details) : base($details) {{}}

                    [Context]ToBlueprint() {{
                        return [DirectoryTemplate]$this.Details
                    }}
                }}

                class {childTemplateName} : Template[FileView] {{
                    {childTemplateName}([IDictionary]$details) : base($details) {{}}

                    [Context]ToBlueprint() {{
                        return [FileTemplate]$this.Details
                    }}
                }}

                $workbench = [View+Workbench]::new([{parentTemplateName}]@{{
                    [Template+DetailOption]::Name = '{parentViewName}'
                    [DirectoryTemplate+DetailOption]::Children = [{childTemplateName}]@{{ [Template+DetailOption]::Name = '{childViewName}' }}
                }})

                $workbench.BuildTo(""{workingDirectoryPath}"")
            ").Invoke().Last().BaseObject;

            Assert.True(Directory.Exists($"{workingDirectoryPath}\\{parentViewName}"));
            Assert.True(File.Exists($"{workingDirectoryPath}\\{parentViewName}\\{childViewName}"));
        }

        [Fact]
        public void BuildTo_WorkingDirectoryFromDynamicCompositionWithMultipleChildrenFiles_ReturnsView()
        {
            using PowerShell instance = PowerShell.Create();

            string workingDirectoryPath = GetWorkingDirectory(nameof(BuildTo_WorkingDirectoryFromDynamicCompositionWithMultipleChildrenFiles_ReturnsView));

            string parentTemplateName, parentViewName;
            parentTemplateName = parentViewName = nameof(parentTemplateName);

            string childTemplateName = nameof(childTemplateName);
            string child1ViewName = nameof(child1ViewName);
            string child2ViewName = nameof(child2ViewName);
            string child3ViewName = nameof(child3ViewName);

            DirectoryView result = (DirectoryView)instance.AddScript($@"
                using module Templata
                using namespace Templata
                using namespace System.Collections

                class {parentTemplateName} : Template[DirectoryView] {{
                    {parentTemplateName}([IDictionary]$details) : base($details) {{}}

                    [Context]ToBlueprint() {{
                        return [DirectoryTemplate]$this.Details
                    }}
                }}

                class {childTemplateName} : Template[FileView] {{
                    {childTemplateName}([IDictionary]$details) : base($details) {{}}

                    [Context]ToBlueprint() {{
                        return [FileTemplate]$this.Details
                    }}
                }}

                $workbench = [View+Workbench]::new([{parentTemplateName}]@{{
                    [Template+DetailOption]::Name = '{parentViewName}'
                    [DirectoryTemplate+DetailOption]::Children = @(
                        [{childTemplateName}]@{{ [Template+DetailOption]::Name = '{child1ViewName}' }},
                        [{childTemplateName}]@{{ [Template+DetailOption]::Name = '{child2ViewName}' }},
                        [{childTemplateName}]@{{ [Template+DetailOption]::Name = '{child3ViewName}' }}
                    )
                }})

                $workbench.BuildTo(""{workingDirectoryPath}"")
            ").Invoke().Last().BaseObject;

            Assert.True(Directory.Exists($"{workingDirectoryPath}\\{parentViewName}"));
            Assert.All(new[] { child1ViewName, child2ViewName, child3ViewName }, childViewName =>
            {
                Assert.True(File.Exists($"{workingDirectoryPath}\\{parentViewName}\\{childViewName}"));
            });
        }

        [Fact]
        public void BuildTo_WorkingDirectoryFromDynamicCompositionWithMultipleChildrenTypes_ReturnsView()
        {
            using PowerShell instance = PowerShell.Create();

            string workingDirectoryPath = GetWorkingDirectory(nameof(BuildTo_WorkingDirectoryFromDynamicCompositionWithMultipleChildrenTypes_ReturnsView));

            string rootTemplateName, rootViewName;
            rootTemplateName = rootViewName = nameof(rootTemplateName);

            string parentTemplateName = nameof(parentTemplateName);
            string parent1ViewName = nameof(parent1ViewName);
            string parent2ViewName = nameof(parent2ViewName);
            string parent3ViewName = nameof(parent3ViewName);

            string childTemplateName = nameof(childTemplateName);
            string child1ViewName = nameof(child1ViewName);
            string child2ViewName = nameof(child2ViewName);
            string child3ViewName = nameof(child3ViewName);

            DirectoryView result = (DirectoryView)instance.AddScript($@"
                using module Templata
                using namespace Templata
                using namespace System.Collections

                class {rootTemplateName} : Template[DirectoryView] {{
                    {rootTemplateName}([IDictionary]$details) : base($details) {{}}

                    [Context]ToBlueprint() {{
                        return [DirectoryTemplate]$this.Details
                    }}
                }}

                class {parentTemplateName} : Template[DirectoryView] {{
                    {parentTemplateName}([IDictionary]$details) : base($details) {{}}

                    [Context]ToBlueprint() {{
                        return [DirectoryTemplate]$this.Details
                    }}
                }}

                class {childTemplateName} : Template[FileView] {{
                    {childTemplateName}([IDictionary]$details) : base($details) {{}}

                    [Context]ToBlueprint() {{
                        return [FileTemplate]$this.Details
                    }}
                }}

                $workbench = [View+Workbench]::new([{rootTemplateName}]@{{
                    [Template+DetailOption]::Name = '{rootViewName}'
                    [DirectoryTemplate+DetailOption]::Children = @(
                        [{parentTemplateName}]@{{
                            [Template+DetailOption]::Name = '{parent1ViewName}'
                            [DirectoryTemplate+DetailOption]::Children = @(
                                [{childTemplateName}]@{{ [Template+DetailOption]::Name = '{child1ViewName}' }},
                                [{childTemplateName}]@{{ [Template+DetailOption]::Name = '{child2ViewName}' }},
                                [{childTemplateName}]@{{ [Template+DetailOption]::Name = '{child3ViewName}' }}
                            )
                        }},
                        [{parentTemplateName}]@{{
                            [Template+DetailOption]::Name = '{parent2ViewName}'
                            [DirectoryTemplate+DetailOption]::Children = @(
                                [{childTemplateName}]@{{ [Template+DetailOption]::Name = '{child1ViewName}' }},
                                [{childTemplateName}]@{{ [Template+DetailOption]::Name = '{child2ViewName}' }},
                                [{childTemplateName}]@{{ [Template+DetailOption]::Name = '{child3ViewName}' }}
                            )
                        }},
                        [{parentTemplateName}]@{{
                            [Template+DetailOption]::Name = '{parent3ViewName}'
                            [DirectoryTemplate+DetailOption]::Children = @(
                                [{childTemplateName}]@{{ [Template+DetailOption]::Name = '{child1ViewName}' }},
                                [{childTemplateName}]@{{ [Template+DetailOption]::Name = '{child2ViewName}' }},
                                [{childTemplateName}]@{{ [Template+DetailOption]::Name = '{child3ViewName}' }}
                            )
                        }}
                    )
                }})

                $workbench.BuildTo(""{workingDirectoryPath}"")
            ").Invoke().Last().BaseObject;

            Assert.True(Directory.Exists($"{workingDirectoryPath}\\{rootViewName}"));
            Assert.All(new[] { parent1ViewName, parent2ViewName, parent3ViewName }, parentViewName =>
            {
                Assert.True(Directory.Exists($"{workingDirectoryPath}\\{rootViewName}\\{parentViewName}"));
                Assert.All(new[] { child1ViewName, child2ViewName, child3ViewName }, childViewName =>
                {
                    Assert.True(File.Exists($"{workingDirectoryPath}\\{rootViewName}\\{parentViewName}\\{childViewName}"));
                });
            });
        }

        [Fact]
        public void BuildTo_WorkingDirectoryFromDynamicCompositionWithMultipleChildrenTypesAndPriorities_ReturnsView()
        {
            using PowerShell instance = PowerShell.Create();

            string workingDirectoryPath = GetWorkingDirectory(nameof(BuildTo_WorkingDirectoryFromDynamicCompositionWithMultipleChildrenTypesAndPriorities_ReturnsView));

            string rootTemplateName, rootViewName;
            rootTemplateName = rootViewName = nameof(rootTemplateName);

            string parentTemplateName = nameof(parentTemplateName);
            string parent1ViewName = nameof(parent1ViewName);
            string parent2ViewName = nameof(parent2ViewName);
            string parent3ViewName = nameof(parent3ViewName);

            string childTemplateName = nameof(childTemplateName);
            string child1ViewName = nameof(child1ViewName);
            string child2ViewName = nameof(child2ViewName);
            string child3ViewName = nameof(child3ViewName);

            DirectoryView result = (DirectoryView)instance.AddScript($@"
                using module Templata
                using namespace Templata
                using namespace System.Collections

                class {rootTemplateName} : Template[DirectoryView] {{
                    {rootTemplateName}([IDictionary]$details) : base($details) {{}}

                    [Context]ToBlueprint() {{
                        return [DirectoryTemplate]$this.Details
                    }}
                }}

                class {parentTemplateName} : Template[DirectoryView] {{
                    {parentTemplateName}([IDictionary]$details) : base($details) {{}}

                    [Context]ToBlueprint() {{
                        return [DirectoryTemplate]$this.Details
                    }}
                }}

                class {childTemplateName} : Template[FileView] {{
                    {childTemplateName}([IDictionary]$details) : base($details) {{}}

                    [Context]ToBlueprint() {{
                        return [FileTemplate]$this.Details
                    }}
                }}

                $workbench = [View+Workbench]::new([{rootTemplateName}]@{{
                    [Template+DetailOption]::Name = '{rootViewName}'
                    [DirectoryTemplate+DetailOption]::Children = @(
                        [{parentTemplateName}]@{{
                            [Template+DetailOption]::Name = '{parent1ViewName}'
                            [Template+DetailOption]::Priority = 1
                            [DirectoryTemplate+DetailOption]::Children = @(
                                [{childTemplateName}]@{{ [Template+DetailOption]::Name = '{child1ViewName}'; [Template+DetailOption]::OnCreated = {{ Start-Sleep -Seconds 1 }} }},
                                [{childTemplateName}]@{{ [Template+DetailOption]::Name = '{child2ViewName}'; [Template+DetailOption]::Priority = -1; [Template+DetailOption]::OnCreated = {{ Start-Sleep -Seconds 1 }} }},
                                [{childTemplateName}]@{{ [Template+DetailOption]::Name = '{child3ViewName}'; [Template+DetailOption]::Priority = 2; [Template+DetailOption]::OnCreated = {{ Start-Sleep -Seconds 1 }} }}
                            )
                            [Template+DetailOption]::OnCreated = {{ Start-Sleep -Seconds 1 }}
                        }},
                        [{parentTemplateName}]@{{
                            [Template+DetailOption]::Name = '{parent2ViewName}'
                            [Template+DetailOption]::Priority = 2
                            [DirectoryTemplate+DetailOption]::Children = @(
                                [{childTemplateName}]@{{ [Template+DetailOption]::Name = '{child1ViewName}'; [Template+DetailOption]::OnCreated = {{ Start-Sleep -Seconds 1 }} }},
                                [{childTemplateName}]@{{ [Template+DetailOption]::Name = '{child2ViewName}'; [Template+DetailOption]::Priority = -1; [Template+DetailOption]::OnCreated = {{ Start-Sleep -Seconds 1 }} }},
                                [{childTemplateName}]@{{ [Template+DetailOption]::Name = '{child3ViewName}'; [Template+DetailOption]::Priority = 2; [Template+DetailOption]::OnCreated = {{ Start-Sleep -Seconds 1 }} }}
                            )
                            [Template+DetailOption]::OnCreated = {{ Start-Sleep -Seconds 1 }}
                        }},
                        [{parentTemplateName}]@{{
                            [Template+DetailOption]::Name = '{parent3ViewName}'
                            [Template+DetailOption]::Priority = 3
                            [DirectoryTemplate+DetailOption]::Children = @(
                                [{childTemplateName}]@{{ [Template+DetailOption]::Name = '{child1ViewName}'; [Template+DetailOption]::OnCreated = {{ Start-Sleep -Seconds 1 }} }},
                                [{childTemplateName}]@{{ [Template+DetailOption]::Name = '{child2ViewName}'; [Template+DetailOption]::Priority = -1; [Template+DetailOption]::OnCreated = {{ Start-Sleep -Seconds 1 }} }},
                                [{childTemplateName}]@{{ [Template+DetailOption]::Name = '{child3ViewName}'; [Template+DetailOption]::Priority = 2; [Template+DetailOption]::OnCreated = {{ Start-Sleep -Seconds 1 }} }}
                            )
                            [Template+DetailOption]::OnCreated = {{ Start-Sleep -Seconds 1 }}
                        }}
                    )
                }})

                $workbench.BuildTo(""{workingDirectoryPath}"")
            ").Invoke().Last().BaseObject;

            DateTime parent1CreationTime = Directory.GetCreationTime($"{workingDirectoryPath}\\{rootViewName}\\{parent1ViewName}");
            DateTime parent2CreationTime = Directory.GetCreationTime($"{workingDirectoryPath}\\{rootViewName}\\{parent2ViewName}");
            DateTime parent3CreationTime = Directory.GetCreationTime($"{workingDirectoryPath}\\{rootViewName}\\{parent3ViewName}");

            Assert.True(parent3CreationTime < parent2CreationTime);
            Assert.True(parent2CreationTime < parent1CreationTime);
            Assert.True(parent3CreationTime < parent1CreationTime);

            Assert.All(new[] { parent1ViewName, parent2ViewName, parent3ViewName }, parentViewName =>
            {
                DateTime child1CreationTime = File.GetCreationTime($"{workingDirectoryPath}\\{rootViewName}\\{parentViewName}\\{child1ViewName}");
                DateTime child2CreationTime = File.GetCreationTime($"{workingDirectoryPath}\\{rootViewName}\\{parentViewName}\\{child2ViewName}");
                DateTime child3CreationTime = File.GetCreationTime($"{workingDirectoryPath}\\{rootViewName}\\{parentViewName}\\{child3ViewName}");

                Assert.True(child3CreationTime < child1CreationTime);
                Assert.True(child1CreationTime < child2CreationTime);
                Assert.True(child3CreationTime < child2CreationTime);
            });
        }

        private WorkingDirectoryFixture Fixture { get; }

        private string GetWorkingDirectory(string testName)
        {
            return Directory.CreateDirectory(Path.Combine(WorkingDirectoryFixture.Path, $"{nameof(DirectoryTemplateTests)}_{testName}")).FullName;
        }
    }

    public sealed partial class WorkbenchTests
    {
        public sealed class InvalidData
        {
            public static IEnumerable<object[]> NonNullNames => Path.GetInvalidPathChars().Where(name => !string.IsNullOrWhiteSpace(name.ToString())).Select(name => new object[] { name.ToString() });
            public static IEnumerable<object[]> NullNames => Path.GetInvalidPathChars().Where(name => string.IsNullOrWhiteSpace(name.ToString())).Select(name => new object[] { name.ToString() });
        }
    }
}
