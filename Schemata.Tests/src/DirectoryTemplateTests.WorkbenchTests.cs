using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Reflection;
using Xunit;

namespace Schemata.Tests;

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
            Model.Workbench workbench = new(template);
            Assert.Throws<ArgumentException>("blueprint", () =>
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
            Model.Workbench workbench = new(template);
            Assert.Throws<ArgumentNullException>("blueprint", () =>
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
        public void BuildTo_WorkingDirectoryFromDynamicComposition_ReturnsModel(string modelName)
        {
            using PowerShell instance = PowerShell.Create();

            int replication = 3;
            string templateName = nameof(templateName);
            string workingDirectoryPath = GetWorkingDirectory(nameof(BuildTo_WorkingDirectoryFromDynamicComposition_ReturnsModel));

            IEnumerable<PSObject> results = instance.AddScript($@"
                using module Schemata
                using namespace Schemata
                using namespace System.Collections

                class {templateName} : Template[DirectoryModel] {{
                    {templateName}([IDictionary]$details) : base($details) {{}}

                    [Blueprint]ToBlueprint() {{
                        return [DirectoryTemplate]$this.Details
                    }}
                }}

                $workbench = [Model+Workbench]::new([{templateName}]@{{ Name = '{modelName}' }})

                1..{replication} | ForEach-Object -Process {{
                    New-Item -Path '{workingDirectoryPath}' -Name $_ -ItemType 'Directory' | Out-Null
                    $workbench.BuildTo(""{workingDirectoryPath}\$_"")
                }}
            ").Invoke().TakeLast(replication);

            Assert.All(Enumerable.Range(1, replication), i =>
            {
                Assert.True(Directory.Exists($"{workingDirectoryPath}\\{i}\\{modelName}"));
                Assert.False(Directory.EnumerateFileSystemEntries($"{workingDirectoryPath}\\{i}\\{modelName}").Any());
            });
            Assert.All(results, result => Assert.IsType<DirectoryModel>(result.BaseObject));
        }

        [Fact]
        public void BuildTo_WorkingDirectoryFromDynamicCompositionWithAChildFile_ReturnsModel()
        {
            using PowerShell instance = PowerShell.Create();

            string workingDirectoryPath = GetWorkingDirectory(nameof(BuildTo_WorkingDirectoryFromDynamicCompositionWithAChildFile_ReturnsModel));

            string parentTemplateName, parentModelName;
            parentTemplateName = parentModelName = nameof(parentTemplateName);

            string childTemplateName, childModelName;
            childTemplateName = childModelName = nameof(childTemplateName);

            DirectoryModel result = (DirectoryModel)instance.AddScript($@"
                using module Schemata
                using namespace Schemata
                using namespace System.Collections

                class {parentTemplateName} : Template[DirectoryModel] {{
                    {parentTemplateName}([IDictionary]$details) : base($details) {{}}

                    [Blueprint]ToBlueprint() {{
                        return [DirectoryTemplate]$this.Details
                    }}
                }}

                class {childTemplateName} : Template[FileModel] {{
                    {childTemplateName}([IDictionary]$details) : base($details) {{}}

                    [Blueprint]ToBlueprint() {{
                        return [FileTemplate]$this.Details
                    }}
                }}

                $workbench = [Model+Workbench]::new([{parentTemplateName}]@{{
                    Name = '{parentModelName}'
                    Children = [{childTemplateName}]@{{ Name = '{childModelName}' }}
                }})

                $workbench.BuildTo(""{workingDirectoryPath}"")
            ").Invoke().Last().BaseObject;

            Assert.True(Directory.Exists($"{workingDirectoryPath}\\{parentModelName}"));
            Assert.True(File.Exists($"{workingDirectoryPath}\\{parentModelName}\\{childModelName}"));
        }

        [Fact]
        public void BuildTo_WorkingDirectoryFromDynamicCompositionWithMultipleChildrenFiles_ReturnsModel()
        {
            using PowerShell instance = PowerShell.Create();

            string workingDirectoryPath = GetWorkingDirectory(nameof(BuildTo_WorkingDirectoryFromDynamicCompositionWithMultipleChildrenFiles_ReturnsModel));

            string parentTemplateName, parentModelName;
            parentTemplateName = parentModelName = nameof(parentTemplateName);

            string childTemplateName = nameof(childTemplateName);
            string child1ModelName = nameof(child1ModelName);
            string child2ModelName = nameof(child2ModelName);
            string child3ModelName = nameof(child3ModelName);

            DirectoryModel result = (DirectoryModel)instance.AddScript($@"
                using module Schemata
                using namespace Schemata
                using namespace System.Collections

                class {parentTemplateName} : Template[DirectoryModel] {{
                    {parentTemplateName}([IDictionary]$details) : base($details) {{}}

                    [Blueprint]ToBlueprint() {{
                        return [DirectoryTemplate]$this.Details
                    }}
                }}

                class {childTemplateName} : Template[FileModel] {{
                    {childTemplateName}([IDictionary]$details) : base($details) {{}}

                    [Blueprint]ToBlueprint() {{
                        return [FileTemplate]$this.Details
                    }}
                }}

                $workbench = [Model+Workbench]::new([{parentTemplateName}]@{{
                    Name = '{parentModelName}'
                    Children = @(
                        [{childTemplateName}]@{{ Name = '{child1ModelName}' }},
                        [{childTemplateName}]@{{ Name = '{child2ModelName}' }},
                        [{childTemplateName}]@{{ Name = '{child3ModelName}' }}
                    )
                }})

                $workbench.BuildTo(""{workingDirectoryPath}"")
            ").Invoke().Last().BaseObject;

            Assert.True(Directory.Exists($"{workingDirectoryPath}\\{parentModelName}"));
            Assert.All(new[] { child1ModelName, child2ModelName, child3ModelName }, childModelName =>
            {
                Assert.True(File.Exists($"{workingDirectoryPath}\\{parentModelName}\\{childModelName}"));
            });
        }

        [Fact]
        public void BuildTo_WorkingDirectoryFromDynamicCompositionWithMultipleChildrenTypes_ReturnsModel()
        {
            using PowerShell instance = PowerShell.Create();

            string workingDirectoryPath = GetWorkingDirectory(nameof(BuildTo_WorkingDirectoryFromDynamicCompositionWithMultipleChildrenTypes_ReturnsModel));

            string rootTemplateName, rootModelName;
            rootTemplateName = rootModelName = nameof(rootTemplateName);

            string parentTemplateName = nameof(parentTemplateName);
            string parent1ModelName = nameof(parent1ModelName);
            string parent2ModelName = nameof(parent2ModelName);
            string parent3ModelName = nameof(parent3ModelName);

            string childTemplateName = nameof(childTemplateName);
            string child1ModelName = nameof(child1ModelName);
            string child2ModelName = nameof(child2ModelName);
            string child3ModelName = nameof(child3ModelName);

            DirectoryModel result = (DirectoryModel)instance.AddScript($@"
                using module Schemata
                using namespace Schemata
                using namespace System.Collections

                class {rootTemplateName} : Template[DirectoryModel] {{
                    {rootTemplateName}([IDictionary]$details) : base($details) {{}}

                    [Blueprint]ToBlueprint() {{
                        return [DirectoryTemplate]$this.Details
                    }}
                }}

                class {parentTemplateName} : Template[DirectoryModel] {{
                    {parentTemplateName}([IDictionary]$details) : base($details) {{}}

                    [Blueprint]ToBlueprint() {{
                        return [DirectoryTemplate]$this.Details
                    }}
                }}

                class {childTemplateName} : Template[FileModel] {{
                    {childTemplateName}([IDictionary]$details) : base($details) {{}}

                    [Blueprint]ToBlueprint() {{
                        return [FileTemplate]$this.Details
                    }}
                }}

                $workbench = [Model+Workbench]::new([{rootTemplateName}]@{{
                    Name = '{rootModelName}'
                    Children = @(
                        [{parentTemplateName}]@{{
                            Name = '{parent1ModelName}'
                            Children = @(
                                [{childTemplateName}]@{{ Name = '{child1ModelName}' }},
                                [{childTemplateName}]@{{ Name = '{child2ModelName}' }},
                                [{childTemplateName}]@{{ Name = '{child3ModelName}' }}
                            )
                        }},
                        [{parentTemplateName}]@{{
                            Name = '{parent2ModelName}'
                            Children = @(
                                [{childTemplateName}]@{{ Name = '{child1ModelName}' }},
                                [{childTemplateName}]@{{ Name = '{child2ModelName}' }},
                                [{childTemplateName}]@{{ Name = '{child3ModelName}' }}
                            )
                        }},
                        [{parentTemplateName}]@{{
                            Name = '{parent3ModelName}'
                            Children = @(
                                [{childTemplateName}]@{{ Name = '{child1ModelName}' }},
                                [{childTemplateName}]@{{ Name = '{child2ModelName}' }},
                                [{childTemplateName}]@{{ Name = '{child3ModelName}' }}
                            )
                        }}
                    )
                }})

                $workbench.BuildTo(""{workingDirectoryPath}"")
            ").Invoke().Last().BaseObject;

            Assert.True(Directory.Exists($"{workingDirectoryPath}\\{rootModelName}"));
            Assert.All(new[] { parent1ModelName, parent2ModelName, parent3ModelName }, parentModelName =>
            {
                Assert.True(Directory.Exists($"{workingDirectoryPath}\\{rootModelName}\\{parentModelName}"));
                Assert.All(new[] { child1ModelName, child2ModelName, child3ModelName }, childModelName =>
                {
                    Assert.True(File.Exists($"{workingDirectoryPath}\\{rootModelName}\\{parentModelName}\\{childModelName}"));
                });
            });
        }

        [Fact]
        public void BuildTo_WorkingDirectoryFromDynamicCompositionWithMultipleChildrenTypesAndPriorities_ReturnsModel()
        {
            using PowerShell instance = PowerShell.Create();

            string workingDirectoryPath = GetWorkingDirectory(nameof(BuildTo_WorkingDirectoryFromDynamicCompositionWithMultipleChildrenTypes_ReturnsModel));

            string rootTemplateName, rootModelName;
            rootTemplateName = rootModelName = nameof(rootTemplateName);

            string parentTemplateName = nameof(parentTemplateName);
            string parent1ModelName = nameof(parent1ModelName);
            string parent2ModelName = nameof(parent2ModelName);
            string parent3ModelName = nameof(parent3ModelName);

            string childTemplateName = nameof(childTemplateName);
            string child1ModelName = nameof(child1ModelName);
            string child2ModelName = nameof(child2ModelName);
            string child3ModelName = nameof(child3ModelName);

            DirectoryModel result = (DirectoryModel)instance.AddScript($@"
                using module Schemata
                using namespace Schemata
                using namespace System.Collections

                class {rootTemplateName} : Template[DirectoryModel] {{
                    {rootTemplateName}([IDictionary]$details) : base($details) {{}}

                    [Blueprint]ToBlueprint() {{
                        return [DirectoryTemplate]$this.Details
                    }}
                }}

                class {parentTemplateName} : Template[DirectoryModel] {{
                    {parentTemplateName}([IDictionary]$details) : base($details) {{}}

                    [Blueprint]ToBlueprint() {{
                        return [DirectoryTemplate]$this.Details
                    }}
                }}

                class {childTemplateName} : Template[FileModel] {{
                    {childTemplateName}([IDictionary]$details) : base($details) {{}}

                    [Blueprint]ToBlueprint() {{
                        return [FileTemplate]$this.Details
                    }}
                }}

                $workbench = [Model+Workbench]::new([{rootTemplateName}]@{{
                    Name = '{rootModelName}'
                    Children = @(
                        [{parentTemplateName}]@{{
                            Name = '{parent1ModelName}'
                            Priority = 1
                            Children = @(
                                [{childTemplateName}]@{{ Name = '{child1ModelName}'; OnCreated = {{ Start-Sleep -Seconds 1 }} }},
                                [{childTemplateName}]@{{ Name = '{child2ModelName}'; Priority = -1; OnCreated = {{ Start-Sleep -Seconds 1 }} }},
                                [{childTemplateName}]@{{ Name = '{child3ModelName}'; Priority = 2; OnCreated = {{ Start-Sleep -Seconds 1 }} }}
                            )
                            OnCreated = {{ Start-Sleep -Seconds 1 }}
                        }},
                        [{parentTemplateName}]@{{
                            Name = '{parent2ModelName}'
                            Priority = 2
                            Children = @(
                                [{childTemplateName}]@{{ Name = '{child1ModelName}'; OnCreated = {{ Start-Sleep -Seconds 1 }} }},
                                [{childTemplateName}]@{{ Name = '{child2ModelName}'; Priority = -1; OnCreated = {{ Start-Sleep -Seconds 1 }} }},
                                [{childTemplateName}]@{{ Name = '{child3ModelName}'; Priority = 2; OnCreated = {{ Start-Sleep -Seconds 1 }} }}
                            )
                            OnCreated = {{ Start-Sleep -Seconds 1 }}
                        }},
                        [{parentTemplateName}]@{{
                            Name = '{parent3ModelName}'
                            Priority = 3
                            Children = @(
                                [{childTemplateName}]@{{ Name = '{child1ModelName}'; OnCreated = {{ Start-Sleep -Seconds 1 }} }},
                                [{childTemplateName}]@{{ Name = '{child2ModelName}'; Priority = -1; OnCreated = {{ Start-Sleep -Seconds 1 }} }},
                                [{childTemplateName}]@{{ Name = '{child3ModelName}'; Priority = 2; OnCreated = {{ Start-Sleep -Seconds 1 }} }}
                            )
                            OnCreated = {{ Start-Sleep -Seconds 1 }}
                        }}
                    )
                }})

                $workbench.BuildTo(""{workingDirectoryPath}"")
            ").Invoke().Last().BaseObject;

            DateTime parent1CreationTime = Directory.GetCreationTime($"{workingDirectoryPath}\\{rootModelName}\\{parent1ModelName}");
            DateTime parent2CreationTime = Directory.GetCreationTime($"{workingDirectoryPath}\\{rootModelName}\\{parent2ModelName}");
            DateTime parent3CreationTime = Directory.GetCreationTime($"{workingDirectoryPath}\\{rootModelName}\\{parent3ModelName}");

            Assert.True(parent3CreationTime < parent2CreationTime);
            Assert.True(parent2CreationTime < parent1CreationTime);
            Assert.True(parent3CreationTime < parent1CreationTime);

            Assert.All(new[] { parent1ModelName, parent2ModelName, parent3ModelName }, parentModelName =>
            {
                DateTime child1CreationTime = File.GetCreationTime($"{workingDirectoryPath}\\{rootModelName}\\{parentModelName}\\{child1ModelName}");
                DateTime child2CreationTime = File.GetCreationTime($"{workingDirectoryPath}\\{rootModelName}\\{parentModelName}\\{child2ModelName}");
                DateTime child3CreationTime = File.GetCreationTime($"{workingDirectoryPath}\\{rootModelName}\\{parentModelName}\\{child3ModelName}");

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
